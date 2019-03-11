using System.Collections.Generic;
using System.Threading.Tasks;

using CryptoCurrency.Core.Currency;
using CryptoCurrency.Core.Exchange.Model;
using CryptoCurrency.Core.Market;
using CryptoCurrency.Core.OrderSide;
using CryptoCurrency.Core.OrderType;
using CryptoCurrency.Core.RateLimiter;
using CryptoCurrency.Core.Symbol;

namespace CryptoCurrency.Core.Exchange
{
    public interface IExchangeHttpClient
    {
        IRateLimiter RateLimiter { get; set; }

        string ApiUrl { get; }

        bool MultiTickSupported { get; }
        
        string InitialTradeFilter { get; }

        void SetApiAccess(string privateKey, string publicKey, string passphrase);

        Task<WrappedResponse<MarketTick>> GetTick(ISymbol symbol);

        Task<WrappedResponse<ICollection<MarketTick>>> GetTicks(ICollection<ISymbol> symbols);

        Task<WrappedResponse<ICollection<AccountBalance>>> GetBalance();

        Task<WrappedResponse<CreateOrder>> CreateOrder(ISymbol symbol, OrderTypeEnum orderType, OrderSideEnum orderSide, decimal price, decimal volume);

        Task<WrappedResponse<TradeFee>> GetTradeFee(OrderSideEnum orderSide, ISymbol symbol);

        Task<WrappedResponse<ICollection<TradeItem>>> GetTradeHistory(ISymbol symbol, int pageNumber, int pageSize, string fromTradeId);

        Task<WrappedResponse<ICollection<OrderItem>>> GetOpenOrders(ISymbol symbol, int pageNumber, int pageSize);

        Task<WrappedResponse<CancelOrder>> CancelOrder(ISymbol symbol, string[] orderIds);

        Task<WrappedResponse<WithdrawCrypto>> WithdrawCrypto(CurrencyCodeEnum cryptoCurrencyCode, decimal withdrawalFee, decimal volume, string address);

        Task<WrappedResponse<ICollection<Deposit>>> GetDeposits(CurrencyCodeEnum currencyCode, int limit);

        Task<WrappedResponse<Deposit>> GetDeposit(CurrencyCodeEnum currencyCode, string transactionId);

        Task<WrappedResponse<TradeResult>> GetTrades(ISymbol symbol, int limit, string filter);

        Task<WrappedResponse<ICollection<ExchangeStats>>> GetStats(ISymbol symbol, ExchangeStatsKeyEnum statsKey);
    }
}
