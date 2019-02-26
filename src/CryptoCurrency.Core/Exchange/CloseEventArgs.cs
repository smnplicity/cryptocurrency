using System;

namespace CryptoCurrency.Core.Exchange
{
    public class CloseEventArgs : EventArgs
    {
        public ushort Code { get; set; }

        public string Reason { get; set; }
    }
}
