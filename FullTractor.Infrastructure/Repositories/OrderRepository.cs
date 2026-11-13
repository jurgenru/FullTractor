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
        return await _fulltractorContext.Orders.AsNoTracking()
            .Where(o => o.Id == orderId)
            .SelectMany(o => o.OrderItems)
            .ToListAsync();
    }

    public async Task<Order?> GetOrderByIdAsync(int orderId)
    {
        return await _fulltractorContext.Orders.AsNoTracking().FirstOrDefaultAsync(o => o.Id == orderId);
    }

    public async Task<Order?> CreateOrderAsync(int userId, IReadOnlyDictionary<int, int> quantities)
    {
        await using var transaction = await _fulltractorContext.Database.BeginTransactionAsync();

        List<int> productIds = [.. quantities.Keys];
        // Sorted by Id so every order locks product rows in the same sequence.
        // Without this, two orders for A+B and B+A can deadlock each other.
        List<Product> products = await _fulltractorContext.Products.AsNoTracking()
            .Where(p => productIds.Contains(p.Id))
            .OrderBy(p => p.Id)
            .ToListAsync();
        if (products.Count != productIds.Count) return null;

        foreach (Product product in products)
        {
            int quantity = quantities[product.Id];
            // Conditional update: only succeeds if there is still enough stock, so concurrent buyers can't oversell.
            int updated = await _fulltractorContext.Products
                .Where(p => p.Id == product.Id && p.Stock >= quantity)
                .ExecuteUpdateAsync(setters => setters.SetProperty(p => p.Stock, p => p.Stock - quantity));
            // Returning without committing rolls back the stock already reduced.
            if (updated == 0) return null;
        }

        Order order = new Order
        {
            UserId = userId,
            OrderDate = DateTime.UtcNow,
            TotalPrice = products.Sum(p => p.Price * quantities[p.Id]),
            OrderItems = [.. products.Select(p => new OrderItem
            {
                ProductId = p.Id,
                Name = p.Name,
                HistoricalPrice = p.Price,
                Quantity = quantities[p.Id]
            })]
        };

        _fulltractorContext.Orders.Add(order);
        await _fulltractorContext.SaveChangesAsync();
        await transaction.CommitAsync();
        return order;
    }

    public async Task<bool> DeleteOrderAsync(int orderId)
    {
        var numDeleted = await _fulltractorContext.Orders.Where(o => o.Id == orderId).ExecuteDeleteAsync();
        if (numDeleted == 0) return false;
        return true;
    }
}
