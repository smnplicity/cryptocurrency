using System;
using System.Threading.Tasks;

using CryptoCurrency.Core.Symbol;

namespace CryptoCurrency.Core.Exchange
{
    public interface IExchangeWebSocketClient
    {
        string Url { get; }

        event EventHandler OnOpen;

        event EventHandler<CloseEventArgs> OnClose;

        event EventHandler<TradesReceivedEventArgs> OnTradesReceived;

        void SetApiAccess(string privateKey, string publicKey, string passphrase);

        Task Begin();

        void Connect();

        void BeginListenTrades(ISymbol symbol);
    }
}
