namespace CryptoCurrency.Core.Currency
{
    public class Gas : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.GAS;
        
        public string Label => "Gas";

        public string Symbol => "GAS";
    }
}
