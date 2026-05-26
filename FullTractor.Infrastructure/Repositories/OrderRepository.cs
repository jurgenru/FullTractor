using FullTractor.Domain.Entities;
using FullTractor.Domain.Interfaces;
using FullTractor.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace FullTractor.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly FullTractorContext _fulltractorContext;
    public OrderRepository(FullTractorContext fullTractorContext)
    {
        _fulltractorContext = fullTractorContext;
    }
    public async Task<List<Order>> GetAllOrdersAsync()
    {
        return await _fulltractorContext.Orders.AsNoTracking().ToListAsync();
    }

    public async Task<List<Order>> GetAllOrdersByUserIdAsync(int userId)
    {
        return await _fulltractorContext.Orders.AsNoTracking().Where(o => o.UserId == userId).ToListAsync();
    }

    public async Task<ICollection<OrderItem>> GetOrderItemsByOrderIdAsync(int orderId)
    {
        return await _fulltractorContext.Orders.AsNoTrackingWithIdentityResolution().Include(o => o.OrderItems.Where(oI => oI.OrderId == orderId)).SelectMany(o => o.OrderItems).ToListAsync();
    }

    public async Task<Order?> GetOrderByIdAsync(int orderId)
    {
        return await _fulltractorContext.Orders.AsNoTracking().FirstOrDefaultAsync(o => o.Id == orderId);
    }
    public async Task<Order> CreateOrderAsync(Order order)
    {
        try
        {
            _fulltractorContext.Orders.Add(order);
            await _fulltractorContext.SaveChangesAsync();
            return order;
        }
        catch (DbUpdateException updateException)
        {
            throw new Exception("Unable to create order. Verify if the properties are correct or the database connection.", updateException);
        }
    }

    public async Task<bool> DeleteOrderAsync(int orderId)
    {
        var numDeleted = await _fulltractorContext.Orders.Where(o => o.Id == orderId).ExecuteDeleteAsync();
        if (numDeleted == 0) return false;
        return true;
    }
}