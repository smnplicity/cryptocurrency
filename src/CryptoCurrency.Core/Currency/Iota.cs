namespace CryptoCurrency.Core.Currency
{
    public class Iota : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.IOTA;

        public string Label => "Iota";

        public string Symbol => "IOTA";
    }
}