using System.Collections.Generic;

namespace CryptoCurrency.Core.Exchange
{
    public interface IExchangeFactory
    {
        IExchange Get(ExchangeEnum exchange);

        ICollection<IExchange> List();
    }
}
