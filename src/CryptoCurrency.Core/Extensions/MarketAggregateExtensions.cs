using System;
using System.Collections.Generic;
using System.Linq;

using CryptoCurrency.Core.Market;

namespace CryptoCurrency.Core.Extensions
{
    public static class MarketAggregateExtensions
    {
        public static double[] GetValues(this ICollection<MarketAggregate> aggregates, CandleTypeEnum candleType)
        {
            switch(candleType)
            {
                case CandleTypeEnum.Open:
                    return aggregates.Select(a => (double)a.Open).ToArray();
                case CandleTypeEnum.High:
                    return aggregates.Select(a => (double)a.High).ToArray();
                case CandleTypeEnum.Low:
                    return aggregates.Select(a => (double)a.Low).ToArray();
                case CandleTypeEnum.Close:
                    return aggregates.Select(a => (double)a.Close).ToArray();
                default:
                    throw new ArgumentException($"Unhandled candle type: {candleType}", "candleType");
            }
        }
    }
}
