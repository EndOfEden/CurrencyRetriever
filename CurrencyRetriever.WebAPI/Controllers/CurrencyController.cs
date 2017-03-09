using System.Web.Http;
using CurrencyRetriever.BusinessEntity;
using log4net;
using System;
using System.Collections.Generic;

namespace CurrencyRetriever.WebAPI.Controllers
{
    public class CurrencyController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ICurrencyService.ICurrencyService _currencyService;

        public CurrencyController(ICurrencyService.ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        /// <summary>
        /// Get Currency Rate between two currencies
        /// </summary>
        [HttpGet]
        [Route("currency/{fromCurrency}/{toCurrency}")]
        public CurrencyRateResult GetCurrencyRate(string fromCurrency, string toCurrency)
        {
            try
            {
                return _currencyService.GetCurrencyRate(fromCurrency, toCurrency);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }
        }

        /// <summary>
        /// Get Currency Rate between two currencies within specified range
        /// </summary>
        [HttpGet]
        [Route("currency/{fromCurrency}/{toCurrency}/{since}/{until}")]
        public IEnumerable<CurrencyRateResult> GetCurrencyRates(string fromCurrency, string toCurrency, DateTime since, DateTime until)
        {
            try
            {
                return _currencyService.GetCurrencyRates(fromCurrency, toCurrency, since, until);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }
        }

        /// <summary>
        /// Get Currency Rate between two currencies on specific date
        /// </summary>
        [HttpGet]
        [Route("currency/{fromCurrency}/{toCurrency}/{date}")]
        public CurrencyRateResult GetCurrencyRate(string fromCurrency, string toCurrency, DateTime date)
        {
            try
            {
                return _currencyService.GetCurrencyRate(fromCurrency, toCurrency, date);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }
        }
    }
}
