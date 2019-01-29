using System.Collections.Generic;

namespace CryptoCurrency.Core.Currency
{
    public interface ICurrencyFactory
    {
        ICurrency Get(CurrencyCodeEnum code);

        ICurrency Get(string code);

        ICollection<ICurrency> List();
    }
}