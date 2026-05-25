using FullTractor.Domain.Entities;

namespace FullTractor.Domain.Interfaces;

public interface IOrderRepository
{
    public Task<List<Order>> GetAllOrdersAsync();
    public Task<List<Order>> GetAllOrdersByUserIdAsync(int userId);
    public Task<Order?> GetOrderByIdAsync(int orderId);
    public Task<Order> CreateOrderAsync(Order order);
    /*LAS ORDER NO SE MODIFICAN, ESTAS TIENEN QUE TENER DATOS PREDECIBLES 
    YA QUE SON INFORMACION EXTERNA DE LOS ORDERITEM QUE CONTIENE ES DECIR NO HAY INFORMACION QUE PUEDA SER MODIFICABLE INTERNAMENTE
    public Task<Order> UpdateOrderAsync(Order order);*/
    public Task<bool> DeleteOrderAsync(int orderId);
}