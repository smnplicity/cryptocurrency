namespace CryptoCurrency.Core.Currency
{
    public class Status : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.SNT;
        
        public string Label => "Status";

        public string Symbol => "SNT";
    }
}
