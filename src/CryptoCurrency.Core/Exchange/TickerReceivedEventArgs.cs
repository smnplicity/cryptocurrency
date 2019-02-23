using System;

using CryptoCurrency.Core.Market;

namespace CryptoCurrency.Core.Exchange
{
    public class TickerReceivedEventArgs : EventArgs
    {
        public MarketTick Data { get; set; }
    }
}