namespace CryptoCurrency.Core.Currency
{
    public class Quantstamp : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.QSP;
        
        public string Label => "Quantstamp";

        public string Symbol => "QSP";
    }
}
