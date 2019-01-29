using System.Collections.Generic;
using System.Threading.Tasks;

using CryptoCurrency.Core.Exchange;
using CryptoCurrency.Core.Interval;
using CryptoCurrency.Core.Market;
using CryptoCurrency.Core.MarketIndicator.Model;
using CryptoCurrency.Core.Symbol;

namespace CryptoCurrency.Core.MarketIndicator
{
    public interface IMarketIndicatorProvider
    {
        Task<ICollection<MovingAverageDataPoint>> Sma(ExchangeEnum exchange, SymbolCodeEnum symbolCode, IntervalKey intervalKey, Epoch from, int dataPoints, CandleTypeEnum candleType, int period);

        Task<ICollection<MovingAverageDataPoint>> Ema(ExchangeEnum exchange, SymbolCodeEnum symbolCode, IntervalKey intervalKey, Epoch from, int dataPoints, CandleTypeEnum candleType, int period);

        Task<ICollection<RsiDataPoint>> Rsi(ExchangeEnum exchange, SymbolCodeEnum symbolCode, IntervalKey intervalKey, Epoch from, int dataPoints, CandleTypeEnum candleType, int rsiPeriod);

        Task<ICollection<MacdDataPoint>> Macd(ExchangeEnum exchange, SymbolCodeEnum symbolCode, IntervalKey intervalKey, Epoch from, int dataPoints, CandleTypeEnum candleType, int fastEmaPeriod, int slowEmaPeriod, int signalPeriod);

        Task<ICollection<BollingerBandsDataPoint>> BollingerBands(ExchangeEnum exchange, SymbolCodeEnum symbolCode, IntervalKey intervalKey, Epoch from, int dataPoints, CandleTypeEnum candleType, int period, MovingAverageTypeEnum maType, double stdDevUp, double stdDevDown);

        Task<ICollection<StochasticDataPoint>> Stochastic(ExchangeEnum exchange, SymbolCodeEnum symbolCode, IntervalKey intervalKey, Epoch from, int dataPoints, MovingAverageTypeEnum kMaType, int kFastPeriod, int kSlowPeriod, MovingAverageTypeEnum dMaType, int dSlowPeriod);
    }
}
