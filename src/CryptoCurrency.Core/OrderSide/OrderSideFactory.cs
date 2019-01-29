using System.Collections.Generic;

namespace CryptoCurrency.Core.OrderSide
{
    public class OrderSideFactory : IOrderSideFactory
    {
        public ICollection<OrderSideEnum> List()
        {
            return new List<OrderSideEnum> { OrderSideEnum.Buy, OrderSideEnum.Sell };
        }
    }
}