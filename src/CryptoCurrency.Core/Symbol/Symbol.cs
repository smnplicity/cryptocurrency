using CryptoCurrency.Core.Currency;

namespace CryptoCurrency.Core.Symbol
{
    public class Symbol : ISymbol
    {
        public SymbolCodeEnum Code { get; private set; }

        public CurrencyCodeEnum BaseCurrencyCode { get; private set; }

        public CurrencyCodeEnum QuoteCurrencyCode { get; private set; }

        public bool Tradable { get; private set; }

        public Symbol(SymbolCodeEnum code, CurrencyCodeEnum baseCurrencyCode, CurrencyCodeEnum quoteCurrencyCode, bool tradable)
        {
            Code = code;
            BaseCurrencyCode = baseCurrencyCode;
            QuoteCurrencyCode = quoteCurrencyCode;
            Tradable = tradable;
        }
    }
}
