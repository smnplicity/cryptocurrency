using System.Threading.Tasks;

namespace CryptoCurrency.Core.OrderSide
{
    public interface IOrderSideRepository
    {
        Task Add(OrderSideEnum orderSide);
    }
}