namespace CryptoCurrency.Core.Currency
{
    public class RaidenNetworkToken : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.RDN;
        
        public string Label => "RaidenNetworkToken";

        public string Symbol => "RDN";
    }
}
