using System;

using CryptoCurrency.Core.Exchange.Model;
using CryptoCurrency.Core.Market;

namespace CryptoCurrency.Core.Exchange
{
    public class TradesReceivedEventArgs : EventArgs
    {
        public TradeResult Data { get; set; }
    }
}