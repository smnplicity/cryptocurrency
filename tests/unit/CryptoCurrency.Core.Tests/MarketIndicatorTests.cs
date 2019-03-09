using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

using CryptoCurrency.Core.Market;
using CryptoCurrency.Core.Exchange;
using CryptoCurrency.Core.Interval;
using CryptoCurrency.Core.Symbol;
using CryptoCurrency.Core.MarketIndicator;
using CryptoCurrency.Core.Interval.Group;

namespace CryptoCurrency.Core.Tests
{
    [TestFixture]
    public class MarketIndicatorTests
    {
        private IIntervalFactory IntervalFactory { get; set; }

        private Mock<IMarketRepository> MarketRepository { get; set; }

        private IMarketIndicatorProvider MarketIndicatorProvider { get; set; }

        [SetUp]
        protected void SetUp()
        {
            var groups = new List<IIntervalGroup>();
            groups.Add(new Minute());
            groups.Add(new Hour());
            groups.Add(new Day());
            groups.Add(new Week());
            groups.Add(new Month());

            IntervalFactory = new IntervalFactory(groups);

            var marketAggregates = JsonConvert.DeserializeObject<ICollection<MarketAggregate>>(File.ReadAllText(Path.Combine("Data", "marketaggregate.json")));

            MarketRepository = new Mock<IMarketRepository>();

            MarketRepository
                .Setup(m => m.GetTradeAggregates(It.IsAny<ExchangeEnum>(), It.IsAny<SymbolCodeEnum>(), It.IsAny<IntervalKey>(), It.IsAny<Epoch>(), It.IsAny<int>()))
                .Returns((ExchangeEnum exchange, SymbolCodeEnum symbolCode, IntervalKey intervalKey, Epoch from, int dataPoints) => 
                {
                    return Task.Run(() =>
                    {
                        return (ICollection<MarketAggregate>)marketAggregates
                            .Where(m => m.Epoch.TimestampMilliseconds >= from.TimestampMilliseconds)
                            .OrderBy(m => m.Epoch.TimestampMilliseconds)
                            .Take(dataPoints)
                            .ToList();
                    });
                });

            MarketIndicatorProvider = new MarketIndicatorProvider(IntervalFactory, MarketRepository.Object);
        }

        [Test]
        public async Task RsiReturnsExpectedValue()
        {
            var intervalKey = new IntervalKey() { IntervalGroup = IntervalGroupEnum.Day, Key = "1D", Duration = 1 };
            var from = new Epoch(new DateTime(2018, 2, 1, 0, 0, 0, DateTimeKind.Utc));
            var dataPoints = 5;

            var rsiValues = await MarketIndicatorProvider.Rsi(ExchangeEnum.Kraken, SymbolCodeEnum.BTCUSD, intervalKey, from, dataPoints, CandleTypeEnum.Close, 14);
            var rsi = rsiValues.First();

            Assert.AreEqual(35.8181, Math.Round(rsi.Rsi, 4));
        }

        [Test]
        public async Task MacdReturnsExpectedValue()
        {
            var intervalKey = new IntervalKey() { IntervalGroup = IntervalGroupEnum.Day, Key = "1D", Duration = 1 };
            var from = new Epoch(new DateTime(2018, 2, 3, 0, 0, 0, DateTimeKind.Utc));
            var dataPoints = 1;

            var macdValues = await MarketIndicatorProvider.Macd(ExchangeEnum.Kraken, SymbolCodeEnum.BTCUSD, intervalKey, from, dataPoints, CandleTypeEnum.Close, 12, 26, 9);
            var macd = macdValues.First();

            Assert.AreEqual(-1550.9828, Math.Round(macd.Macd, 4));
            Assert.AreEqual(-1561.9757, Math.Round(macd.Signal, 4));
            Assert.AreEqual(10.9928, Math.Round(macd.Histogram, 4));
        }
    }
}
