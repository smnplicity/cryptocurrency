namespace CryptoCurrency.Core.Currency
{
    public class BitcoinDiamond : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.BCD;
        
        public string Label => "BitcoinDiamond";

        public string Symbol => "BCD";
    }
}
