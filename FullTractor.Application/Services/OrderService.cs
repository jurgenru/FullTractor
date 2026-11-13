using FullTractor.Application.DTOs;
using FullTractor.Application.Interfaces;
using FullTractor.Domain.Entities;
using FullTractor.Domain.Interfaces;
using FullTractor.Application.Enums;
using FullTractor.Application.DTOs.Service.Response;

namespace FullTractor.Application.Services;

public class OrderService : IOrderService
{
    public IOrderRepository _orderRepository;
    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    public async Task<ServiceResponse<List<OrderResponse>>> GetAllOrderByUserIdsAsync(int userId)
    {
        List<Order> order = await _orderRepository.GetAllOrdersByUserIdAsync(userId);
        if (order.Count > 0) return ConvertListOrderResponse(order);
        return new ServiceResponse<List<OrderResponse>> { Status = Status.NotFound };
    }

    public async Task<ServiceResponse<OrderResponse>> GetOrderByIdAsync(int id)
    {
        Order? order = await _orderRepository.GetOrderByIdAsync(id);
        if (order != null) return ConvertOrderResponse(order);
        return new ServiceResponse<OrderResponse> { Status = Status.NotFound };
    }

    public async Task<ServiceResponse<List<OrderItemResponse>>> GetOrderItemByOrderIdAsync(int id)
    {
        ICollection<OrderItem> orderItemList = await _orderRepository.GetOrderItemsByOrderIdAsync(id);
        if (orderItemList.Count > 0) return ConvertListOrderItemResponse(orderItemList);
        return new ServiceResponse<List<OrderItemResponse>> { Status = Status.OrderItemNotExist };
    }

    public async Task<ServiceResponse<OrderResponse>> CreateOrderAsync(int userId, CreateOrderRequest createOrderRequest)
    {
        // Merge repeated products so stock is checked against the total quantity.
        Dictionary<int, int> quantities = createOrderRequest.Items
            .GroupBy(i => i.ProductId)
            .ToDictionary(g => g.Key, g => g.Sum(i => i.Quantity));

        Order? orderCreated = await _orderRepository.CreateOrderAsync(userId, quantities);
        if (orderCreated == null) return new ServiceResponse<OrderResponse> { Status = Status.InsufficientStock };
        return ConvertOrderResponse(orderCreated);
    }

    public async Task<ServiceResponse<OrderResponse>> DeleteOrderAsync(int id)
    {
        ServiceResponse<OrderResponse> orderExist = await GetOrderByIdAsync(id);
        if (orderExist.Status == Status.NotFound) return orderExist;
        bool orderDeleted = await _orderRepository.DeleteOrderAsync(id);
        if (!orderDeleted) return new ServiceResponse<OrderResponse> { Status = Status.DeleteError };
        return new ServiceResponse<OrderResponse> { Status = Status.Success };
    }

    private ServiceResponse<List<OrderResponse>> ConvertListOrderResponse(List<Order> order)
    {
        return new ServiceResponse<List<OrderResponse>> { Status = Status.Success, Data = [.. order.Select(o => new OrderResponse { Id = o.Id, UserId = o.UserId, OrderDate = o.OrderDate, TotalPrice = o.TotalPrice })] };
    }
    private ServiceResponse<OrderResponse> ConvertOrderResponse(Order order)
    {
        return new ServiceResponse<OrderResponse> { Status = Status.Success, Data = new OrderResponse { Id = order.Id, UserId = order.UserId, OrderDate = order.OrderDate, TotalPrice = order.TotalPrice } };
    }
    private ServiceResponse<List<OrderItemResponse>> ConvertListOrderItemResponse(ICollection<OrderItem> orderItems)
    {
        return new ServiceResponse<List<OrderItemResponse>> { Status = Status.Success, Data = [.. orderItems.Select(oI => new OrderItemResponse { OrderItemId = oI.Id, Name = oI.Name, HistoricalPrice = oI.HistoricalPrice, ProductId = oI.ProductId, Quantity = oI.Quantity })] };
    }
}