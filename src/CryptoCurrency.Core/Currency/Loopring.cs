namespace CryptoCurrency.Core.Currency
{
    public class Loopring : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.LRC;
        
        public string Label => "Loopring";

        public string Symbol => "LRC";
    }
}
