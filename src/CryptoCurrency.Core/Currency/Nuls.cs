namespace CryptoCurrency.Core.Currency
{
    public class Nuls : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.NULS;
        
        public string Label => "Nuls";

        public string Symbol => "NULS";
    }
}
