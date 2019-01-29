using System;
using System.Collections.Generic;
using System.Linq;

using CryptoCurrency.Core.Exchange;

namespace CryptoCurrency.ExchangeClient
{
    public class ExchangeFactory : IExchangeFactory
    {
        private static ICollection<IExchange> Exchanges { get; set; }

        public ExchangeFactory(IEnumerable<IExchange> exchanges)
        {
            Exchanges = exchanges.ToList();
        }

        public IExchange Get(ExchangeEnum exchange)
        {
            var match = Exchanges.Where(e => e.Name == exchange).FirstOrDefault();

            if (match == null)
                throw new ArgumentException($"Unable to find exchange '{exchange}'");

            return match;
        }

        public ICollection<IExchange> List()
        {
            return Exchanges;
        }
    }
}
