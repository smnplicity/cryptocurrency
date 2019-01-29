using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using static TicTacTec.TA.Library.Core;

using CryptoCurrency.Core.Exchange;
using CryptoCurrency.Core.Extensions;
using CryptoCurrency.Core.Interval;
using CryptoCurrency.Core.Market;
using CryptoCurrency.Core.MarketIndicator.Model;
using CryptoCurrency.Core.Symbol;

namespace CryptoCurrency.Core.MarketIndicator
{
    public class MarketIndicatorProvider : IMarketIndicatorProvider
    {
        private IIntervalFactory IntervalFactory { get; set; }

        private IMarketRepository MarketRepository { get; set; }

        public MarketIndicatorProvider(IIntervalFactory intervalFactory, IMarketRepository marketRepository)
        {
            IntervalFactory = intervalFactory;

            MarketRepository = marketRepository;
        }

        public async Task<ICollection<MovingAverageDataPoint>> Sma(ExchangeEnum exchange, SymbolCodeEnum symbolCode, IntervalKey intervalKey, Epoch from, int dataPoints, CandleTypeEnum candleType, int period)
        {
            var periodOffset = period - 1;

            var fromOffset = IntervalFactory.GetInterval(intervalKey, from, periodOffset * -1);

            var aggValues = await MarketRepository.GetTradeAggregates(exchange, symbolCode, intervalKey, fromOffset.From, periodOffset + dataPoints);

            var values = aggValues.GetValues(candleType);

            int outBegIdx, outNbElement;

            var smaValues = new double[dataPoints];

            var retCode = TicTacTec.TA.Library.Core.Sma(0, values.Length - 1, values, period, out outBegIdx, out outNbElement, smaValues);

            var validSmaValues = smaValues.Skip(outNbElement - dataPoints).Take(dataPoints).ToArray();

            var validAggValues = aggValues.Skip(aggValues.Count - dataPoints).Take(dataPoints).ToArray();

            if (retCode == RetCode.Success)
            {
                var dp = new List<MovingAverageDataPoint>();

                for (var i = 0; i < validAggValues.Length; i++)
                {
                    var agg = validAggValues[i];

                    dp.Add(new MovingAverageDataPoint
                    {
                        Epoch = agg.Epoch,
                        Value = validSmaValues[i]
                    });
                }

                return dp;
            }

            throw new Exception("Unable to calculate SMA - " + retCode);
        }

        public async Task<ICollection<MovingAverageDataPoint>> Ema(ExchangeEnum exchange, SymbolCodeEnum symbolCode, IntervalKey intervalKey, Epoch from, int dataPoints, CandleTypeEnum candleType, int period)
        {
            var periodOffset = period - 1;

            var fromOffset = IntervalFactory.GetInterval(intervalKey, from, periodOffset * -1);

            var aggValues = await MarketRepository.GetTradeAggregates(exchange, symbolCode, intervalKey, fromOffset.From, periodOffset + dataPoints);

            var values = aggValues.GetValues(candleType);

            int outBegIdx, outNbElement;

            var smaValues = new double[dataPoints];

            var retCode = TicTacTec.TA.Library.Core.Ema(0, values.Length - 1, values, period, out outBegIdx, out outNbElement, smaValues);

            var validSmaValues = smaValues.Skip(outNbElement - dataPoints).Take(dataPoints).ToArray();

            var validAggValues = aggValues.Skip(aggValues.Count - dataPoints).Take(dataPoints).ToArray();

            if (retCode == RetCode.Success)
            {
                var dp = new List<MovingAverageDataPoint>();

                for (var i = 0; i < validAggValues.Length; i++)
                {
                    var agg = validAggValues[i];

                    dp.Add(new MovingAverageDataPoint
                    {
                        Epoch = agg.Epoch,
                        Value = validSmaValues[i]
                    });
                }

                return dp;
            }

            throw new Exception("Unable to calculate EMA - " + retCode);
        }

        public async Task<ICollection<RsiDataPoint>> Rsi(ExchangeEnum exchange, SymbolCodeEnum symbolCode, IntervalKey intervalKey, Epoch from, int dataPoints, CandleTypeEnum candleType, int rsiPeriod)
        {
            var fromOffset = IntervalFactory.GetInterval(intervalKey, from, rsiPeriod * -1);

            var aggValues = await MarketRepository.GetTradeAggregates(exchange, symbolCode, intervalKey, fromOffset.From, rsiPeriod + dataPoints);

            var values = aggValues.GetValues(candleType);

            int outBegIdx, outNbElement;

            var rsiValues = new double[dataPoints];

            var retCode = TicTacTec.TA.Library.Core.Rsi(0, values.Length - 1, values, rsiPeriod, out outBegIdx, out outNbElement, rsiValues);

            var validRsiValues = rsiValues.Skip(outNbElement - dataPoints).Take(dataPoints).ToArray();
            
            var validAggValues = aggValues.Skip(aggValues.Count - dataPoints).Take(dataPoints).ToArray();

            if (retCode == RetCode.Success)
            {
                var dp = new List<RsiDataPoint>();

                for (var i = 0; i < validAggValues.Length; i++)
                {
                    var agg = validAggValues[i];

                    dp.Add(new RsiDataPoint
                    {
                        Epoch = agg.Epoch,
                        Rsi = validRsiValues[i]
                    });
                }

                return dp;
            }

            throw new Exception("Unable to calculate RSI - " + retCode);
        }

        public async Task<ICollection<MacdDataPoint>> Macd(ExchangeEnum exchange, SymbolCodeEnum symbolCode, IntervalKey intervalKey, Epoch from, int dataPoints, CandleTypeEnum candleType, int fastEmaPeriod, int slowEmaPeriod, int signalPeriod)
        {
            var periodOffset = (slowEmaPeriod + signalPeriod) - 2;

            var fromOffset = IntervalFactory.GetInterval(intervalKey, from, periodOffset * -1);

            var aggValues = await MarketRepository.GetTradeAggregates(exchange, symbolCode, intervalKey, fromOffset.From, periodOffset + dataPoints);

            var values = aggValues.GetValues(candleType);

            int outBegIdx, outNbElement;

            var macdValues = new double[dataPoints];
            var signalValues = new double[dataPoints];
            var histogramValues = new double[dataPoints];

            var retCode = TicTacTec.TA.Library.Core.Macd(0, values.Length - 1, values, fastEmaPeriod, slowEmaPeriod, signalPeriod, out outBegIdx, out outNbElement, macdValues, signalValues, histogramValues);

            var validMacdValues = macdValues.Skip(outNbElement - dataPoints).Take(dataPoints).ToArray();
            var validSignalValues = signalValues.Skip(outNbElement - dataPoints).Take(dataPoints).ToArray();
            var validHistogramValues = histogramValues.Skip(outNbElement - dataPoints).Take(dataPoints).ToArray();
            var validAggValues = aggValues.Skip(aggValues.Count - dataPoints).Take(dataPoints).ToArray();

            if (retCode == RetCode.Success)
            {
                var dp = new List<MacdDataPoint>();

                for (var i = 0; i < validAggValues.Length; i++)
                {
                    var agg = validAggValues[i];

                    dp.Add(new MacdDataPoint
                    {
                        Epoch = agg.Epoch,
                        Macd = validMacdValues[i],
                        Signal = validSignalValues[i],
                        Histogram = validHistogramValues[i]
                    });
                }

                return dp;
            }

            throw new Exception("Unable to calculate MACD - " + retCode);
        }

        public async Task<ICollection<BollingerBandsDataPoint>> BollingerBands(ExchangeEnum exchange, SymbolCodeEnum symbolCode, IntervalKey intervalKey, Epoch from, int dataPoints, CandleTypeEnum candleType, int period, MovingAverageTypeEnum maType, double stdDevUp, double stdDevDown)
        {
            var maTypeConverted = maType.ToTaLib();

            var periodOffset = period - 1;

            var fromOffset = IntervalFactory.GetInterval(intervalKey, from, periodOffset * -1);

            var aggValues = await MarketRepository.GetTradeAggregates(exchange, symbolCode, intervalKey, fromOffset.From, periodOffset + dataPoints);

            var values = aggValues.GetValues(candleType);

            int outBegIdx, outNbElement;

            var upperBandValues = new double[dataPoints];
            var middleBandValues = new double[dataPoints];
            var lowerBandValues = new double[dataPoints];

            var retCode = Bbands(0, values.Length - 1, values, period, stdDevUp, stdDevDown, maTypeConverted, out outBegIdx, out outNbElement, upperBandValues, middleBandValues, lowerBandValues);

            var validUpperBandValues = upperBandValues.Skip(outNbElement - dataPoints).Take(dataPoints).ToArray();
            var validMiddleBandValues = middleBandValues.Skip(outNbElement - dataPoints).Take(dataPoints).ToArray();
            var validLowerBandValues = lowerBandValues.Skip(outNbElement - dataPoints).Take(dataPoints).ToArray();

            var validAggValues = aggValues.Skip(aggValues.Count - dataPoints).Take(dataPoints).ToArray();

            if (retCode == RetCode.Success)
            {
                var dp = new List<BollingerBandsDataPoint>();

                for (var i = 0; i < validAggValues.Length; i++)
                {
                    var agg = validAggValues[i];

                    dp.Add(new BollingerBandsDataPoint
                    {
                        Epoch = agg.Epoch,
                        Upper = upperBandValues[i],
                        Middle = middleBandValues[i],
                        Lower = lowerBandValues[i]
                    });
                }

                return dp;
            }

            throw new Exception("Unable to calculate Bollinger Bands - " + retCode);
        }

        public async Task<ICollection<StochasticDataPoint>> Stochastic(ExchangeEnum exchange, SymbolCodeEnum symbolCode, IntervalKey intervalKey, Epoch from, int dataPoints, MovingAverageTypeEnum kMaType, int kFastPeriod, int kSlowPeriod, MovingAverageTypeEnum dMaType, int dSlowPeriod)
        {
            var dMaTypeConverted = dMaType.ToTaLib();
            var kMaTypeConverted = kMaType.ToTaLib();

            var periodOffset = kSlowPeriod + 1;

            var fromOffset = IntervalFactory.GetInterval(intervalKey, from, periodOffset * -1);

            var aggValues = await MarketRepository.GetTradeAggregates(exchange, symbolCode, intervalKey, fromOffset.From, periodOffset + dataPoints);

            var highPoints = aggValues.GetValues(CandleTypeEnum.High);
            var lowPoints = aggValues.GetValues(CandleTypeEnum.Low);
            var closePoints = aggValues.GetValues(CandleTypeEnum.Close);

            int outBegIdx, outNbElement;

            var kValues = new double[dataPoints];
            var dValues = new double[dataPoints];

            var retCode = Stoch(0, closePoints.Length - 1, highPoints, lowPoints, closePoints, kFastPeriod, kSlowPeriod, kMaTypeConverted, dSlowPeriod, dMaTypeConverted, out outBegIdx, out outNbElement, kValues, dValues);

            var validKValues = kValues.Skip(outNbElement - dataPoints).Take(dataPoints).ToArray();
            var validDValues = dValues.Skip(outNbElement - dataPoints).Take(dataPoints).ToArray();

            var validAggValues = aggValues.Skip(aggValues.Count - dataPoints).Take(dataPoints).ToArray();

            if (retCode == RetCode.Success)
            {
                var dp = new List<StochasticDataPoint>();

                for (var i = 0; i < validAggValues.Length; i++)
                {
                    var agg = validAggValues[i];

                    dp.Add(new StochasticDataPoint
                    {
                        Epoch = agg.Epoch,
                        D = validKValues[i],
                        K = validDValues[i],
                    });
                }

                return dp;
            }

            throw new Exception("Unable to calculate Stochastic - " + retCode);
        }
    }
}