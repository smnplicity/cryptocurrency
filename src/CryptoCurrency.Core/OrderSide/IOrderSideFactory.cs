using System.Collections.Generic;

namespace CryptoCurrency.Core.OrderSide
{
    public interface IOrderSideFactory
    {
        ICollection<OrderSideEnum> List();
    }
}