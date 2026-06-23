using FullTractor.Application.DTOs;
using FullTractor.Application.DTOs.Service;
using FullTractor.Application.DTOs.Service.Response;

namespace FullTractor.Application.Interfaces;

public interface IOrderService
{
    public Task<ServiceResponse<List<OrderResponse>>> GetAllOrderByUserIdsAsync(int userId);
    public Task<ServiceResponse<OrderResponse>> GetOrderByIdAsync(int id);
    public Task<ServiceResponse<List<OrderItemResponse>>> GetOrderItemByOrderIdAsync(int id);
    public Task<ServiceResponse<OrderResponse>> CreateOrderAsync(CreateOrderRequest createOrderRequest);
    public Task<ServiceResponse<OrderResponse>> DeleteOrderAsync(int id);
}