namespace CryptoCurrency.Core.Currency
{
    public class BlockMason : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.BCPT;
        
        public string Label => "BlockMason";

        public string Symbol => "BCPT";
    }
}
