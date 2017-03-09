using CurrencyRetriever.BusinessEntity;
using System;
using System.Collections.Generic;

namespace CurrencyRetriever.ICurrencyService
{
    public interface ICurrencyService
    {
        CurrencyRateResult GetCurrencyRate(string fromCurrency, string toCurrency);
        IEnumerable<CurrencyRateResult> GetCurrencyRates(string fromCurrency, string toCurrency, DateTime since, DateTime until);
        CurrencyRateResult GetCurrencyRate(string fromCurrency, string toCurrency, DateTime date);
    }
}
