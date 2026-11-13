using FullTractor.Domain.Entities;

namespace FullTractor.Domain.Interfaces;

public interface IOrderRepository
{
    public Task<List<Order>> GetAllOrdersAsync();
    public Task<List<Order>> GetAllOrdersByUserIdAsync(int userId);
    public Task<Order?> GetOrderByIdAsync(int orderId);
    public Task<ICollection<OrderItem>> GetOrderItemsByOrderIdAsync(int orderId);
    /// <summary>
    /// Creates an order for the user, pricing it from the database and reducing stock in one transaction.
    /// Returns null when a product does not exist or does not have enough stock.
    /// </summary>
    /// <param name="quantities">Quantity requested per product id.</param>
    public Task<Order?> CreateOrderAsync(int userId, IReadOnlyDictionary<int, int> quantities);
    /*LAS ORDER NO SE MODIFICAN, ESTAS TIENEN QUE TENER DATOS PREDECIBLES 
    YA QUE SON INFORMACION EXTERNA DE LOS ORDERITEM QUE CONTIENE ES DECIR NO HAY INFORMACION QUE PUEDA SER MODIFICABLE INTERNAMENTE
    public Task<Order> UpdateOrderAsync(Order order);*/
    public Task<bool> DeleteOrderAsync(int orderId);
}