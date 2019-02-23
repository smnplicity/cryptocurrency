namespace CryptoCurrency.Core.Currency
{
    public class AirSwap : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.AST;
        
        public string Label => "AirSwap";

        public string Symbol => "AST";
    }
}
