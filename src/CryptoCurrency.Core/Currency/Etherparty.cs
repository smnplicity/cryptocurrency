namespace CryptoCurrency.Core.Currency
{
    public class Etherparty : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.FUEL;
        
        public string Label => "Etherparty";

        public string Symbol => "FUEL";
    }
}
