namespace CryptoCurrency.Core.Currency
{
    public class PowerLedger : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.POWR;
        
        public string Label => "PowerLedger";

        public string Symbol => "POWR";
    }
}
