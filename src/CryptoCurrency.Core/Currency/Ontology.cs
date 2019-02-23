namespace CryptoCurrency.Core.Currency
{
    public class Ontology : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.ONT;
        
        public string Label => "Ontology";

        public string Symbol => "ONT";
    }
}
