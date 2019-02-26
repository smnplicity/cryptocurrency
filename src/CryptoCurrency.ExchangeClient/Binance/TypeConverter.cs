using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

using CryptoCurrency.Core;
using CryptoCurrency.Core.Currency;
using CryptoCurrency.Core.Exchange.Model;
using CryptoCurrency.Core.Extensions;
using CryptoCurrency.Core.Market;
using CryptoCurrency.Core.OrderSide;
using CryptoCurrency.Core.OrderType;
using CryptoCurrency.Core.Symbol;

using CryptoCurrency.ExchangeClient.Binance.Model;

namespace CryptoCurrency.ExchangeClient.Binance
{
    public static class TypeConverter
    {
        public static async Task<T2> ChangeType<T, T2>(this Binance exchange, ICurrencyFactory currencyFactory, ISymbolFactory symbolFactory, NameValueCollection postData, T obj)
        {
            if (typeof(T) == typeof(ICollection<BinancePriceTicker>))
            {
                var info = await exchange.GetExchangeInfo();

                var ticks = new List<MarketTick>();

                var priceTicks = obj as ICollection<BinancePriceTicker>;

                foreach(var tick in priceTicks)
                {
                    var binanceSymbol = info.Symbols.Where(x => x.Symbol == tick.Symbol).FirstOrDefault();

                    var baseCurrencyCode = exchange.GetStandardisedCurrencyCode(currencyFactory, binanceSymbol.BaseAsset);
                    var quoteCurrencyCode = exchange.GetStandardisedCurrencyCode(currencyFactory, binanceSymbol.QuoteAsset);
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

                    var baseCurrencyCode = exchange.GetStandardisedCurrencyCode(currencyFactory, binanceSymbol.BaseAsset);
                    var quoteCurrencyCode = exchange.GetStandardisedCurrencyCode(currencyFactory, binanceSymbol.QuoteAsset);
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

            if (typeof(T2) == typeof(TradeResult))
            {
                var info = await exchange.GetExchangeInfo();

                var trades = obj as ICollection<Dictionary<string, object>>;

                var filter = trades.Count > 0 ? trades.Last()["a"].ToString() : postData["fromId"];

                var binanceSymbol = info.Symbols.Where(x => x.Symbol == postData["symbol"]).FirstOrDefault();

                var baseCurrencyCode = exchange.GetStandardisedCurrencyCode(currencyFactory, binanceSymbol.BaseAsset);
                var quoteCurrencyCode = exchange.GetStandardisedCurrencyCode(currencyFactory, binanceSymbol.QuoteAsset);
                var symbol = symbolFactory.Get(baseCurrencyCode, quoteCurrencyCode);

                return (T2)(object)new TradeResult
                {
                    Exchange = exchange.Name,
                    SymbolCode = symbol.Code,                    
                    Trades = trades.Select(t => new MarketTrade
                    {
                        Exchange = exchange.Name,
                        SymbolCode = symbol.Code,
                        Epoch = Epoch.FromMilliseconds(Convert.ToInt64(t["T"])),
                        Price = Convert.ToDouble(t["p"]),
                        Volume = Convert.ToDouble(t["q"]),
                        Side = Convert.ToBoolean(t["m"]) ? OrderSideEnum.Sell : OrderSideEnum.Buy,
                        SourceTradeId = Convert.ToString(t["a"]).ToString()
                    }).ToList(),
                    Filter = filter
                };
            }

            if(typeof(T) == typeof(ICollection<BinanceTradeItem>))
            {
                var info = await exchange.GetExchangeInfo();

                var binanceSymbol = info.Symbols.Where(x => x.Symbol == postData["symbol"]).FirstOrDefault();

                var baseCurrencyCode = exchange.GetStandardisedCurrencyCode(currencyFactory, binanceSymbol.BaseAsset);
                var quoteCurrencyCode = exchange.GetStandardisedCurrencyCode(currencyFactory, binanceSymbol.QuoteAsset);

                var symbol = symbolFactory.Get(baseCurrencyCode, quoteCurrencyCode);

                var trades = obj as ICollection<BinanceTradeItem>;

                return (T2)(object)trades.Select(t => new TradeItem
                {
                    Exchange = exchange.Name,
                    SymbolCode = symbol.Code,
                    Created = Epoch.FromMilliseconds(t.Time),
                    Id = t.Id.ToString(),
                    OrderId = t.OrderId.ToString(),
                    Fee = t.Commission,
                    FeeCurrencyCode = exchange.GetStandardisedCurrencyCode(currencyFactory, t.CommissionAsset),
                    Price = t.Price,
                    Volume = t.Quantity,
                    Side = t.IsBuyer ? OrderSideEnum.Buy : OrderSideEnum.Sell
                }).ToList();
            }

            if(typeof(T) == typeof(BinanceNewOrder))
            {
                var info = await exchange.GetExchangeInfo();

                var binanceSymbol = info.Symbols.Where(x => x.Symbol == postData["symbol"]).FirstOrDefault();

                var baseCurrencyCode = exchange.GetStandardisedCurrencyCode(currencyFactory, binanceSymbol.BaseAsset);
                var quoteCurrencyCode = exchange.GetStandardisedCurrencyCode(currencyFactory, binanceSymbol.QuoteAsset);

                var symbol = symbolFactory.Get(baseCurrencyCode, quoteCurrencyCode);

                var newOrder = obj as BinanceNewOrder;

                var orderType = exchange.GetOrderType(newOrder.Type);

                var price = orderType == OrderTypeEnum.Limit ? newOrder.Price : newOrder.Fills.Average(f => f.Price);

                return (T2)(object)new CreateOrder
                {
                    Exchange = exchange.Name,
                    SymbolCode = symbol.Code,
                    Id = newOrder.OrderId,
                    Side = exchange.GetOrderSide(newOrder.Side),
                    Type = orderType,
                    State = exchange.GetOrderState(newOrder.Status),
                    OrderEpoch = Epoch.FromMilliseconds(newOrder.TransactTime),
                    Price = price,
                    Volume = newOrder.OriginalQuantity
                }; 
            }

            if(typeof(T) == typeof(ICollection<BinanceOpenOrder>))
            {
                var info = await exchange.GetExchangeInfo();

                var binanceSymbol = info.Symbols.Where(x => x.Symbol == postData["symbol"]).FirstOrDefault();

                var baseCurrencyCode = exchange.GetStandardisedCurrencyCode(currencyFactory, binanceSymbol.BaseAsset);
                var quoteCurrencyCode = exchange.GetStandardisedCurrencyCode(currencyFactory, binanceSymbol.QuoteAsset);

                var symbol = symbolFactory.Get(baseCurrencyCode, quoteCurrencyCode);

                var openOrders = obj as ICollection<BinanceOpenOrder>;

                return (T2)(object)openOrders.Select(o => new OrderItem
                {
                    Exchange = exchange.Name,
                    SymbolCode = symbol.Code,
                    Id = o.OrderId,
                    Side = exchange.GetOrderSide(o.Side),
                    Type = exchange.GetOrderType(o.Type),
                    State = exchange.GetOrderState(o.Status),
                    OrderEpoch = Epoch.FromMilliseconds(o.Time),
                    Price = o.Price,
                    AvgPrice = o.Price,
                    Volume = o.OriginalQuantity,
                    RemainingVolume = o.OriginalQuantity - o.ExecutedQuantity
                }).ToList();
            }

            if(typeof(T) == typeof(BinanceCancelOrder))
            {
                var cancelOrder = obj as BinanceCancelOrder;

                return (T2)(object)new CancelOrder
                {
                    Exchange = exchange.Name,
                    Id = cancelOrder.OrderId,
                    Epoch = Epoch.FromMilliseconds(cancelOrder.TransactTime),
                    State = exchange.GetOrderState(cancelOrder.Status)
                };
            }

            if (typeof(T) == typeof(T2))
                return (T2)(object)obj;

            throw new Exception("Invalid type provided - " + typeof(T2));
        }
    }
}
