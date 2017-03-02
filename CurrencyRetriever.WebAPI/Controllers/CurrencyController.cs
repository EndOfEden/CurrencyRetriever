using System.Web.Http;
using CurrencyRetriever.BusinessEntity;
using log4net;
using System;

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
        public CurrencyRateBE GetCurrencyRate(string fromCurrency, string toCurrency)
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
    }
}
