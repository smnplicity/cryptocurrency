using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

using CryptoCurrency.Core;
using CryptoCurrency.Core.Exchange.Model;
using CryptoCurrency.Core.Extensions;
using CryptoCurrency.Core.Market;
using CryptoCurrency.Core.OrderSide;
using CryptoCurrency.Core.Symbol;

using CryptoCurrency.ExchangeClient.CoinbasePro.Model;

namespace CryptoCurrency.ExchangeClient.CoinbasePro
{
    public static class TypeConverter
    {
        public static T2 ChangeType<T, T2>(this CoinbasePro exchange, ISymbolFactory symbolFactory, object requestData, T obj, long? cbBefore)
        {
            if (typeof(T) == typeof(CoinbaseProCancelOrder))
                return (T2)(object)new CancelOrder();

            if (typeof(T) == typeof(CoinbaseProCreateOrderResponse))
            {
                var order = obj as CoinbaseProCreateOrderResponse;

                var symbol = exchange.DecodeProductId(order.ProductId);

                return (T2)(object)new CreateOrder
                {
                    Id = order.Id,
                    SymbolCode = symbol.Code,
                    Side = exchange.GetOrderSide(order.Side),
                    Type = exchange.GetOrderType(order.Type),
                    OrderTime = order.CreatedAt.ToUniversalTime(),
                    Price = order.Price,
                    Volume = order.Size,
                    State = exchange.GetOrderState(order.Status)
                };
            }

            if (typeof(T) == typeof(ICollection<CoinbaseProAccount>))
            {
                var account = obj as ICollection<CoinbaseProAccount>;

                return (T2)(object)account.Select(a => new AccountBalance
                {
                    CurrencyCode = exchange.GetStandardisedCurrencyCode(a.Currency),
                    Balance = a.Balance,
                    PendingFunds = a.Hold
                }).ToList();
            }

            if (typeof(T) == typeof(ICollection<CoinbaseProOrder>))
            {
                var orders = obj as ICollection<CoinbaseProOrder>;

                return (T2)(object)orders.Select(o => new OrderItem
                {
                    Id = o.Id,
                    SymbolCode = exchange.DecodeProductId(o.ProductId).Code,
                    Side = exchange.GetOrderSide(o.Side),
                    Type = exchange.GetOrderType(o.Type),
                    Price = o.Price,
                    Volume = o.Size,
                    AvgPrice = o.Price,
                    RemainingVolume = o.Size - o.FilledSize,
                    Fee = o.FillFees,
                    State = exchange.GetOrderState(o.Status)
                }).ToList();
            }

            if (typeof(T) == typeof(CoinbaseProOrderBook))
            {
                var orderBook = obj as CoinbaseProOrderBook;

                var nvc = requestData as NameValueCollection;

                return (T2)(object)new OrderBook
                {
                    Exchange = exchange.Name,
                    SymbolCode = exchange.DecodeProductId(nvc["ProductId"]).Code,
                    Ask = orderBook.Asks.Select(a => new OrderBookItem
                    {
                        Side = OrderSideEnum.Buy,
                        Price = a.ElementAt(0),
                        AvgPrice = a.ElementAt(0),
                        Volume = a.ElementAt(1)
                    }).ToList(),
                    Bid = orderBook.Bids.Select(a => new OrderBookItem
                    {
                        Side = OrderSideEnum.Sell,
                        Price = a.ElementAt(0),
                        AvgPrice = a.ElementAt(0),
                        Volume = a.ElementAt(1)
                    }).ToList()
                };
            }

            if (typeof(T) == typeof(CoinbaseProTick))
            {
                var tick = obj as CoinbaseProTick;

                var nvc = requestData as NameValueCollection;

                return (T2)(object)new MarketTick
                {
                    Exchange = exchange.Name,
                    SymbolCode = exchange.DecodeProductId(nvc["ProductId"]).Code,
                    Epoch = new Epoch(tick.Time.ToUniversalTime()),
                    BuyPrice = tick.Ask,
                    SellPrice = tick.Bid,
                    LastPrice = tick.Price
                };
            }

            if (typeof(T) == typeof(ICollection<CoinbaseProFill>))
            {
                var trades = obj as ICollection<CoinbaseProFill>;

                return (T2)(object)trades.Select(t => new TradeItem
                {
                    Exchange = exchange.Name,
                    SymbolCode = exchange.DecodeProductId(t.ProductId).Code,
                    Id = t.TradeId,
                    OrderId = t.OrderId,
                    Created = new Epoch(t.CreatedAt.ToUniversalTime()),
                    Side = exchange.GetOrderSide(t.Side),
                    Price = t.Price,
                    Volume = t.Size,
                    Fee = t.Fee
                }).ToList();
            }

            if (typeof(T) == typeof(ICollection<CoinbaseProMarketTrade>))
            {
                var trades = obj as ICollection<CoinbaseProMarketTrade>;

                var data = (Dictionary<string, object>)requestData;

                var symbolCode = (SymbolCodeEnum)data["SymbolCode"];
                var limit = (int)data["Limit"];

                return (T2)(object)new TradeResult
                {
                    Exchange = exchange.Name,
                    SymbolCode = symbolCode,
                    Filter = cbBefore.HasValue ? (cbBefore.Value + limit).ToString() : null,
                    Trades = trades.Select(t => new MarketTrade
                    {
                        Exchange = exchange.Name,
                        SymbolCode = symbolCode,
                        Epoch = new Epoch(t.Time),
                        Price = t.Price,
                        Volume = t.Size,
                        Side = exchange.GetOrderSide(t.Side),
                        SourceTradeId = t.TradeId.ToString()
                    }).ToList()
                };
            }

            throw new Exception("Invalid type provided - " + typeof(T2));
        }
    }
}
