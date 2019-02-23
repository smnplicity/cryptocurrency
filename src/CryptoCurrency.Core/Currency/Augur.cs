namespace CryptoCurrency.Core.Currency
{
    public class Augur : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.REP;
        
        public string Label => "Augur";

        public string Symbol => "REP";
    }
}
