using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

using CryptoCurrency.Core;
using CryptoCurrency.Core.Exchange.Model;
using CryptoCurrency.Core.Extensions;
using CryptoCurrency.Core.Market;
using CryptoCurrency.Core.Symbol;

using CryptoCurrency.ExchangeClient.Binance.Model;

namespace CryptoCurrency.ExchangeClient.Binance
{
    public static class TypeConverter
    {
        public static async Task<T2> ChangeType<T, T2>(this Binance exchange, ISymbolFactory symbolFactory, NameValueCollection postData, T obj)
        {
            if (typeof(T) == typeof(ICollection<BinancePriceTicker>))
            {
                var info = await exchange.GetExchangeInfo();

                var ticks = new List<MarketTick>();

                var priceTicks = obj as ICollection<BinancePriceTicker>;

                foreach(var tick in priceTicks)
                {
                    var binanceSymbol = info.Symbols.Where(x => x.Symbol == tick.Symbol).FirstOrDefault();

                    var baseCurrencyCode = exchange.GetStandardisedCurrencyCode(binanceSymbol.BaseAsset);
                    var quoteCurrencyCode = exchange.GetStandardisedCurrencyCode(binanceSymbol.QuoteAsset);
                    var symbol = symbolFactory.Get(baseCurrencyCode, quoteCurrencyCode);

                    ticks.Add(new MarketTick
                    {
                        Exchange = exchange.Name,
                        SymbolCode = symbol.Code,
                        Epoch = Epoch.Now,
                        LastPrice = tick.Price,
                    });
                }

                return (T2)(object)ticks;
            }

            if (typeof(T) == typeof(ICollection<BinanceOrderBookTicker>))
            {
                var info = await exchange.GetExchangeInfo();

                var ticks = new List<MarketTick>();

                var bookTicks = (obj as ICollection<BinanceOrderBookTicker>);

                foreach(var tick in bookTicks)
                {
                    var binanceSymbol = info.Symbols.Where(x => x.Symbol == tick.Symbol).FirstOrDefault();

                    var baseCurrencyCode = exchange.GetStandardisedCurrencyCode(binanceSymbol.BaseAsset);
                    var quoteCurrencyCode = exchange.GetStandardisedCurrencyCode(binanceSymbol.QuoteAsset);
                    var symbol = symbolFactory.Get(baseCurrencyCode, quoteCurrencyCode);

                    ticks.Add(new MarketTick
                    {
                        Exchange = exchange.Name,
                        SymbolCode = symbol.Code,
                        Epoch = Epoch.Now,
                        BuyPrice = tick.AskPrice,
                        SellPrice = tick.BidPrice
                    });
                }

                return (T2)(object)ticks;
            }

            if (typeof(T) == typeof(ICollection<BinanceAggTrade>))
            {
                var info = await exchange.GetExchangeInfo();

                var trades = obj as ICollection<BinanceAggTrade>;

                var filter = trades.Count > 0 ? trades.Last().TradeId.ToString() : postData["fromId"];

                var binanceSymbol = info.Symbols.Where(x => x.Symbol == postData["symbol"]).FirstOrDefault();

                var baseCurrencyCode = exchange.GetStandardisedCurrencyCode(binanceSymbol.BaseAsset);
                var quoteCurrencyCode = exchange.GetStandardisedCurrencyCode(binanceSymbol.QuoteAsset);
                var symbol = symbolFactory.Get(baseCurrencyCode, quoteCurrencyCode);

                return (T2)(object)new TradeResult
                {
                    Exchange = exchange.Name,
                    SymbolCode = symbol.Code,                    
                    Trades = trades.Select(t => new MarketTrade
                    {
                        Exchange = exchange.Name,
                        SymbolCode = symbol.Code,
                        Epoch = Epoch.FromMilliseconds(t.Timestamp),
                        Price = t.Price,
                        Volume = t.Volume,
                        SourceTradeId = t.TradeId.ToString()
                    }).ToList(),
                    Filter = filter
                };
            }

            if (typeof(T) == typeof(T2))
                return (T2)(object)obj;

            throw new Exception("Invalid type provided - " + typeof(T2));
        }
    }
}
