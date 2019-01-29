namespace CryptoCurrency.Core.Currency
{
    public class Litecoin : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.LTC;
        
        public string Label => "Litecoin";
  
        public string Symbol => "Ł";
    }
}