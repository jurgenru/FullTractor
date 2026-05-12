using FullTractor.Domain.Entities;

namespace FullTractor.Domain.Interfaces;

public interface IOrderRepository
{
    public Task<List<Order>> GetAllOrdersAsync();
    public Task<List<Order>> GetAllOrdersByUserIdAsync(int userId);
    public Task<Order?> GetOrderByIdAsync(int orderId);
    public Task<Order> CreateOrderAsync(Order order);
    public Task<Order> UpdateOrderAsync(Order order);
    public Task<bool> DeleteOrderAsync(int orderId);
}