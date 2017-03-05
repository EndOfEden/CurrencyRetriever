namespace CurrencyRetriever.BusinessEntity
{
    public class CurrencyRateBE
    {
        public int Id { get; set; }
        public int FromCurrency { get; set; }
        public int ToCurrency { get; set; }
        public decimal Rate { get; set; }
    }

    public class CurrencyRateResult
    {
        public decimal Rate { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
    }
}
