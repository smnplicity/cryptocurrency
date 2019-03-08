using CryptoCurrency.Core.Currency;

namespace CryptoCurrency.Core.Symbol
{
    public class Symbol : ISymbol
    {
        public SymbolCodeEnum Code { get; set; }

        public CurrencyCodeEnum BaseCurrencyCode { get; set; }

        public CurrencyCodeEnum QuoteCurrencyCode { get; set; }

        public bool Tradable { get; private set; }

        public Symbol()
        {

        }

        public Symbol(SymbolCodeEnum code, CurrencyCodeEnum baseCurrencyCode, CurrencyCodeEnum quoteCurrencyCode, bool tradable)
        {
            Code = code;
            BaseCurrencyCode = baseCurrencyCode;
            QuoteCurrencyCode = quoteCurrencyCode;
            Tradable = tradable;
        }
    }
}
