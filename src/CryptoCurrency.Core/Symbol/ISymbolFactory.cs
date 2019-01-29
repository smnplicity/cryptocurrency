using System.Collections.Generic;

using CryptoCurrency.Core.Currency;

namespace CryptoCurrency.Core.Symbol
{
    public interface ISymbolFactory
    {
        ICollection<ISymbol> List();

        ICollection<ISymbol> ListTradable();

        ISymbol Get(CurrencyCodeEnum baseCurrencyCode, CurrencyCodeEnum quoteCurrencyCode);

        ISymbol Get(SymbolCodeEnum symbolCode);
    }
}