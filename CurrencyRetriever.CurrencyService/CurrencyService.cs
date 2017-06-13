using Dapper;
using System;
using CurrencyRetriever.BusinessEntity;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Collections.Generic;

namespace CurrencyRetriever.CurrencyService
{
    public class CurrencyService : ICurrencyService.ICurrencyService
    {
        public CurrencyRateResult GetCurrencyRate(string fromCurrency, string toCurrency)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["CurrencyAppDB"].ConnectionString))
                {
                    sqlConnection.Open();

                    var p = new DynamicParameters();
                    p.Add("@fromCurrency", fromCurrency);
                    p.Add("@toCurrency", toCurrency);

                    CurrencyRateResult currencyRates = sqlConnection.Query<CurrencyRateResult>("GetLatestCurrencyRate", p, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    return currencyRates;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<CurrencyBE> GetCurrencyList()
        {
            try
            {
                using (var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["CurrencyAppDB"].ConnectionString))
                {
                    sqlConnection.Open();

                    IEnumerable<CurrencyBE> currencyList = sqlConnection.Query<CurrencyBE>("SELECT Id, Name, Code, Country FROM Currency");

                    return currencyList;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<CurrencyRateResult> GetCurrencyRates(string fromCurrency, string toCurrency, DateTime since, DateTime until)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["CurrencyAppDB"].ConnectionString))
                {
                    sqlConnection.Open();

                    var p = new DynamicParameters();
                    p.Add("@fromCurrency", fromCurrency);
                    p.Add("@toCurrency", toCurrency);
                    p.Add("@since", since);
                    p.Add("@until", until);

                    IEnumerable<CurrencyRateResult> currencyRates = sqlConnection.Query<CurrencyRateResult>("GetLatestCurrencyRateRange", p, commandType: CommandType.StoredProcedure);

                    return currencyRates;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CurrencyRateResult GetCurrencyRate(string fromCurrency, string toCurrency, DateTime date)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["CurrencyAppDB"].ConnectionString))
                {
                    sqlConnection.Open();

                    var p = new DynamicParameters();
                    p.Add("@fromCurrency", fromCurrency);
                    p.Add("@toCurrency", toCurrency);
                    p.Add("@date", date);

                    CurrencyRateResult currencyRate = sqlConnection.Query<CurrencyRateResult>("GetCurrencyRateOnDate", p, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    return currencyRate;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
