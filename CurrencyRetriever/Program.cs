using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Globalization;
using log4net;
using System.Reflection;
using CurrencyRetriever.BusinessEntity;

namespace CurrencyRetriever
{
    class Program
    {
        static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            bool errorFound = false;

            do
            {
                try
                {
                    Execute();

                    //Console.WriteLine(responseText);
                    log.Info($"Program completed execution at {DateTime.Now}");

                    errorFound = false;
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    errorFound = true;
                    System.Threading.Thread.Sleep(60000);

#if DEBUG
                    Console.WriteLine(ex.Message);
#endif
                }
            } while (errorFound);

#if DEBUG
            Console.WriteLine("Press Enter to exit");
            Console.ReadLine();
#endif
        }

        static void Execute()
        {
            using (var sqlConnection = new SqlConnection(Properties.Settings.Default.CurrencyApp_DB))
            {
                sqlConnection.Open();

                IEnumerable<CurrencyBE> currencies = sqlConnection.Query<CurrencyBE>("SELECT * FROM Currency");

                foreach (CurrencyBE currency in currencies)
                {
                    // 2. Retrieve currency rate one by one (https://api.fixer.io/latest?base={currencyRate})
                    HttpClient httpClient = new HttpClient();
                    httpClient.BaseAddress = new Uri(Properties.Settings.Default.CurrencyRate_BaseURL);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

                    //DateTime dateTime = DateTime.Now.Date.AddDays(-1);
                    DateTime dateTime = new DateTime(2017, 5, 4);

                    while (dateTime.Date < DateTime.Now.Date)
                    {
                        try
                        {
                            dateTime = dateTime.AddDays(1);
                            string dateTimeString = dateTime.ToString("yyyy-MM-dd");

                            var response = httpClient.GetAsync($"latest/{dateTimeString}?base={currency.Code}").Result;

                            //var response = httpClient.GetAsync($"latest?base={currency.Code}").Result;
                            string responseText = response.Content.ReadAsStringAsync().Result;

                            RateResultBE rateResult = JsonConvert.DeserializeObject<RateResultBE>(responseText);

                            CurrencyBE fromCurrency = currencies.FirstOrDefault(x => x.Code == rateResult.Base);

                            if (fromCurrency == null)
                            {
                                var fromCodeParam = new DynamicParameters();
                                fromCodeParam.Add("@code", rateResult.Base);

                                fromCurrency = sqlConnection.Query<CurrencyBE>("InsertCurrency", fromCodeParam, commandType: CommandType.StoredProcedure).FirstOrDefault();
                            }

                            foreach (var rate in rateResult.Rates)
                            {
                                CurrencyBE toCurrency = currencies.FirstOrDefault(x => x.Code == rate.Key);

                                if (toCurrency == null)
                                {
                                    var toCodeParam = new DynamicParameters();
                                    toCodeParam.Add("@code", rate.Key);

                                    toCurrency = sqlConnection.Query<CurrencyBE>("InsertCurrency", toCodeParam, commandType: CommandType.StoredProcedure).FirstOrDefault();
                                }

                                DateTime rateDate = DateTime.ParseExact(rateResult.Date, "yyyy-MM-dd", null);
                                decimal actualRate = 0;

                                if (!Decimal.TryParse(rate.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out actualRate))
                                {
                                    log.Error($"Unable to convert value {rate.Value} to decimal.");
#if DEBUG
                                    Console.WriteLine($"Unable to convert value {rate.Value} to decimal.");
#endif
                                }

                                var rateParam = new DynamicParameters();
                                rateParam.Add("@fromCurrency", fromCurrency.Id);
                                rateParam.Add("@toCurrency", toCurrency.Id);
                                rateParam.Add("@rate", actualRate);
                                rateParam.Add("@year", rateDate.Year);
                                rateParam.Add("@month", rateDate.Month);
                                rateParam.Add("@day", rateDate.Day);

                                sqlConnection.Execute("UpsertCurrencyRateHistory", rateParam, commandType: CommandType.StoredProcedure);
                            }
#if DEBUG
                            Console.WriteLine(responseText);
#endif
                        }
                        catch
                        {
                            dateTime = dateTime.AddDays(-1);
                            continue;
                        }
                    }
                }
            }
        }
    }
}
