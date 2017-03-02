using CurrencyRetriever.BusinessEntity;

namespace CurrencyRetriever.ICurrencyService
{
    public interface ICurrencyService
    {
        CurrencyRateBE GetCurrencyRate(string fromCurrency, string toCurrency);
    }
}
