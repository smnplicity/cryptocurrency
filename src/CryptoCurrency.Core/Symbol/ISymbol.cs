using CryptoCurrency.Core.Currency;

namespace CryptoCurrency.Core.Symbol
{
    public interface ISymbol
    {
        SymbolCodeEnum Code { get; }

        CurrencyCodeEnum BaseCurrencyCode { get; }

        CurrencyCodeEnum QuoteCurrencyCode { get; }

        bool Tradable { get; }
    }
}