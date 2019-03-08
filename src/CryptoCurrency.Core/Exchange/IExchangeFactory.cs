using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoCurrency.Core.Exchange
{
    public interface IExchangeFactory
    {
        Task<IExchange> Get(ExchangeEnum exchange);

        ICollection<IExchange> List();
    }
}
