namespace CryptoCurrency.Core.Currency
{
    public class ETHLend : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.LEND;
        
        public string Label => "ETHLend";

        public string Symbol => "LEND";
    }
}
