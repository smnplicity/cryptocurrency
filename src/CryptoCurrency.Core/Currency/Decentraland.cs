namespace CryptoCurrency.Core.Currency
{
    public class Decentraland : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.MANA;
        
        public string Label => "Decentraland";

        public string Symbol => "MANA";
    }
}
