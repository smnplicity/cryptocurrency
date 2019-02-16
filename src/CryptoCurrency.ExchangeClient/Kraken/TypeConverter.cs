using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

using CryptoCurrency.Core;
using CryptoCurrency.Core.Currency;
using CryptoCurrency.Core.Extensions;
using CryptoCurrency.Core.Exchange.Model;
using CryptoCurrency.Core.Market;
using CryptoCurrency.Core.OrderType;
using CryptoCurrency.Core.OrderSide;
using CryptoCurrency.Core.OrderState;
using CryptoCurrency.Core.Symbol;

using CryptoCurrency.ExchangeClient.Kraken.Model;

namespace CryptoCurrency.ExchangeClient.Kraken
{
    public static class TypeConverter
    {
        public static async Task<T2> ChangeType<T, T2>(this Kraken exchange, ISymbolFactory symbolFactory, NameValueCollection postData, T obj)
        {
            if (typeof(T) == typeof(KrakenCancelOrderResult))
            {
                var cancelOrder = obj as KrakenCancelOrderResult;

                return (T2)(object)new CancelOrder();
            }

            if (typeof(T) == typeof(KrakenOrderQuery))
            {
                var orders = obj as KrakenOrderQuery;
                
                return (T2)(object)orders.Select(kvp => new OrderItem
                {
                    SymbolCode = symbolFactory.Get(exchange.DecodeAssetPair(kvp.Value.Descr.Pair)[0], exchange.DecodeAssetPair(kvp.Value.Descr.Pair)[1]).Code,
                    Id = kvp.Key,
                    OrderDateTime = Epoch.FromSeconds((long)kvp.Value.OpenTm).DateTime,
                    Side = exchange.GetOrderSide(kvp.Value.Descr.Type),
                    Type = exchange.GetOrderType(kvp.Value.Descr.OrderType),
                    State = exchange.GetOrderState(kvp.Value.Status),
                    Price = exchange.GetOrderType(kvp.Value.Descr.OrderType) == OrderTypeEnum.Limit ? kvp.Value.LimitPrice : kvp.Value.Price,
                    AvgPrice = kvp.Value.Price,
                    Fee = kvp.Value.Fee,
                    Volume = kvp.Value.Volume,
                    RemainingVolume = kvp.Value.Volume - kvp.Value.VolumeExec
                });
            }

            if (typeof(T) == typeof(KrakenOpenOrders))
            {
                var orders = obj as KrakenOpenOrders;

                return (T2)(object)orders.Open.Where(o => o.Value.Descr.Pair == postData["pair"]).Select(kvp => new OrderItem
                {
                    SymbolCode = symbolFactory.Get(exchange.DecodeAssetPair(kvp.Value.Descr.Pair)[0], exchange.DecodeAssetPair(kvp.Value.Descr.Pair)[1]).Code,
                    Id = kvp.Key,
                    OrderDateTime = Epoch.FromSeconds((long)kvp.Value.OpenTm).DateTime,
                    Side = exchange.GetOrderSide(kvp.Value.Descr.Type),
                    Type = exchange.GetOrderType(kvp.Value.Descr.OrderType),
                    State = exchange.GetOrderState(kvp.Value.Status),
                    Price = exchange.GetOrderType(kvp.Value.Descr.OrderType) == OrderTypeEnum.Limit ? kvp.Value.LimitPrice : kvp.Value.Price,
                    AvgPrice = kvp.Value.Price,
                    Fee = kvp.Value.Fee,
                    Volume = kvp.Value.Volume,
                    RemainingVolume = kvp.Value.Volume - kvp.Value.VolumeExec
                }).ToList();
            }

            if (typeof(T) == typeof(KrakenClosedOrders))
            {
                var orders = obj as KrakenClosedOrders;

                return (T2)(object)orders.Closed.Where(o => o.Value.Descr.Pair == postData["pair"]).Select(kvp => new OrderItem
                {
                    SymbolCode = symbolFactory.Get(exchange.DecodeAssetPair(kvp.Value.Descr.Pair)[0], exchange.DecodeAssetPair(kvp.Value.Descr.Pair)[1]).Code,
                    Id = kvp.Key,
                    OrderDateTime = Epoch.FromSeconds((long)kvp.Value.OpenTm).DateTime,
                    Side = exchange.GetOrderSide(kvp.Value.Descr.Type),
                    Type = exchange.GetOrderType(kvp.Value.Descr.OrderType),
                    State = exchange.GetOrderState(kvp.Value.Status),
                    Price = exchange.GetOrderType(kvp.Value.Descr.OrderType) == OrderTypeEnum.Limit ? kvp.Value.LimitPrice : kvp.Value.Price,
                    AvgPrice = kvp.Value.Price,
                    Fee = kvp.Value.Fee,
                    Volume = kvp.Value.Volume,
                    RemainingVolume = kvp.Value.Volume - kvp.Value.VolumeExec
                }).ToList();
            }

            if (typeof(T) == typeof(KrakenAddOrderResult))
            {
                var order = obj as KrakenAddOrderResult;

                return (T2)(object)new CreateOrder
                {
                    Id = order.TxId.First(),
                    OrderTime = DateTime.UtcNow,
                    SymbolCode = symbolFactory.Get(exchange.DecodeAssetPair(postData["pair"])[0], exchange.DecodeAssetPair(postData["pair"])[1]).Code,
                    Side = exchange.GetOrderSide(postData["type"]),
                    Type = exchange.GetOrderType(postData["ordertype"]),
                    Price = Convert.ToDouble(postData["price"]),
                    Volume = Convert.ToDouble(postData["volume"]),
                    State = OrderStateEnum.Pending
                };
            }

            if (typeof(T) == typeof(KrakenAccount))
            {
                var balance = obj as KrakenAccount;

                var assets = await exchange.GetAssets();

                return (T2)(object)balance.Select(kvp => new AccountBalance
                {
                    CurrencyCode = exchange.GetStandardisedCurrencyCode(assets[kvp.Key].AltName),
                    Balance = kvp.Value
                }).ToList();
            }

            if (typeof(T) == typeof(KrakenTradeHistory))
            {
                var assets = await exchange.GetAssets();

                var assetPairs = await exchange.GetAssetPairs();

                var trades = obj as KrakenTradeHistory;

                return (T2)(object)trades.Trades
                    .Where(kvp => postData["pair"] == $"{exchange.GetCurrencyCode(exchange.DecodeQuotePair(kvp.Value.Pair)[0])}{exchange.GetCurrencyCode(exchange.DecodeQuotePair(kvp.Value.Pair)[1])}")
                    .Select(kvp => new TradeItem
                    {
                        Exchange = exchange.Name,
                        SymbolCode = symbolFactory.Get(exchange.GetStandardisedCurrencyCode(assets[assetPairs[kvp.Value.Pair].Base].AltName), exchange.GetStandardisedCurrencyCode(assets[assetPairs[kvp.Value.Pair].Quote].AltName)).Code,
                        Id = kvp.Key,
                        OrderId = kvp.Value.OrderTxId,
                        Created = Epoch.FromSeconds(kvp.Value.Time),
                        Side = exchange.GetOrderSide(kvp.Value.Type),
                        Price = kvp.Value.Price,
                        Fee = kvp.Value.Fee,
                        Volume = kvp.Value.Volume
                    }).ToList();
            }

            if (typeof(T) == typeof(KrakenTick))
            {
                var assets = await exchange.GetAssets();

                var assetPairs = await exchange.GetAssetPairs();

                var tick = (obj as KrakenTick).First();

                return (T2)(object)new MarketTick
                {
                    Exchange = exchange.Name,
                    SymbolCode = symbolFactory.Get(exchange.GetStandardisedCurrencyCode(assets[assetPairs[tick.Key].Base].AltName), exchange.GetStandardisedCurrencyCode(assets[assetPairs[tick.Key].Quote].AltName)).Code,
                    Epoch = Epoch.Now,
                    BuyPrice = tick.Value.Ask.First(),
                    SellPrice = tick.Value.Bid.First(),
                    LastPrice = tick.Value.Last.First()
                };
            }

            if (typeof(T) == typeof(KrakenTicks))
            {
                var assets = await exchange.GetAssets();

                var assetPairs = await exchange.GetAssetPairs();

                var ticks = obj as KrakenTicks;

                return (T2)(object)ticks.Select(kvp => new MarketTick
                {
                    Exchange = exchange.Name,
                    SymbolCode = symbolFactory.Get(exchange.GetStandardisedCurrencyCode(assets[assetPairs[kvp.Key].Base].AltName), exchange.GetStandardisedCurrencyCode(assets[assetPairs[kvp.Key].Quote].AltName)).Code,
                    Epoch = Epoch.Now,
                    BuyPrice = kvp.Value.Ask.First(),
                    SellPrice = kvp.Value.Bid.First(),
                    LastPrice = kvp.Value.Last.First()
                }).ToList();
            }

            if (typeof(T) == typeof(KrakenOrderBook))
            {
                var book = (obj as KrakenOrderBook).First();

                var pair = exchange.DecodeAssetPair(postData["pair"]);

                var symbol = symbolFactory.Get(pair[0], pair[1]);

                return (T2)(object)new OrderBook
                {
                    Exchange = exchange.Name,
                    SymbolCode = symbol.Code,
                    Ask = book.Value.Asks.Select(a => new OrderBookItem
                    {
                        Price = a.ElementAt(0),
                        AvgPrice = a.ElementAt(0),
                        Volume = a.ElementAt(1),
                        OrderTime = Epoch.FromSeconds((long)a.ElementAt(2)).DateTime
                    }).ToList(),
                    Bid = book.Value.Bids.Select(a => new OrderBookItem
                    {
                        Price = a.ElementAt(0),
                        Volume = a.ElementAt(1),
                        OrderTime = Epoch.FromSeconds((long)a.ElementAt(2)).DateTime
                    }).ToList()
                };
            }

            if (typeof(T) == typeof(KrakenTradeVolume))
            {
                var tradeVol = (obj as KrakenTradeVolume);

                var takerFee = tradeVol.Fees.First().Value.Fee;

                return (T2)(object)new TradeFee
                {
                    CurrencyCode = CurrencyCodeEnum.USD,
                    Taker = takerFee / 100.0,
                    Maker = (tradeVol.FeesMaker != null && tradeVol.FeesMaker.Count > 0 ? tradeVol.FeesMaker.First().Value.Fee : takerFee) / 100.0
                };
            }

            if (typeof(T) == typeof(KrakenTrade<string, List<List<object>>>))
            {
                var tradeResult = (obj as KrakenTrade<string, List<List<object>>>);

                var pair = exchange.DecodeAssetPair(postData["pair"]);

                var symbol = symbolFactory.Get(pair[0], pair[1]);

                return (T2)(object)new TradeResult
                {
                    Exchange = exchange.Name,
                    SymbolCode = symbol.Code,
                    Filter = Convert.ToString(tradeResult.Last),
                    Trades = tradeResult.First().Value.Select(t => new MarketTrade
                    {
                        Exchange = exchange.Name,
                        SymbolCode = symbol.Code,
                        Epoch = Epoch.FromSeconds(Convert.ToDouble(t[2])),
                        Price = Convert.ToDouble(t[0]),
                        Volume = Convert.ToDouble(t[1]),
                        Side = Convert.ToString(t[3]) == "b" ? OrderSideEnum.Buy : OrderSideEnum.Sell
                    }).ToList()
                };
            }

            if (typeof(T) == typeof(T2))
                return (T2)(object)obj;

            throw new Exception("Invalid type provided - " + typeof(T2));
        }
    }
}
