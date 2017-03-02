using Dapper;
using System;
using CurrencyRetriever.BusinessEntity;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Linq;

namespace CurrencyRetriever.CurrencyService
{
    public class CurrencyService : ICurrencyService.ICurrencyService
    {
        public CurrencyRateBE GetCurrencyRate(string fromCurrency, string toCurrency)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["CurrencyAppDB"].ConnectionString))
                {
                    sqlConnection.Open();

                    var p = new DynamicParameters();
                    p.Add("@fromCurrency", fromCurrency);
                    p.Add("@toCurrency", toCurrency);

                    CurrencyRateBE currencyRates = sqlConnection.Query<CurrencyRateBE>("GetLatestCurrencyRate", p, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    return currencyRates;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
