using FullTractor.Api.Extensions;
using FullTractor.Application.DTOs;
using FullTractor.Application.DTOs.Service.Response;
using FullTractor.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FullTractor.Api.Controllers;

[Route("[controller]")]
[Authorize]
public class OrderController : ApiControllerBase
{
    private readonly IOrderService _orderService;
    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet("{userId}/allOrders")]
    public async Task<ActionResult<ServiceResponse<List<OrderResponse>>>> GetAllOrderByUserIdsAsync([FromRoute] int userId)
    {
        if (!User.CanAccessUser(userId)) return Forbid();
        return ToResponse(await _orderService.GetAllOrderByUserIdsAsync(userId));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceResponse<OrderResponse>>> GetOrderByIdAsync([FromRoute] int id)
    {
        ServiceResponse<OrderResponse> orderById = await _orderService.GetOrderByIdAsync(id);
        if (orderById.Data != null && !User.CanAccessUser(orderById.Data.UserId)) return Forbid();
        return ToResponse(orderById);
    }

    [HttpGet("{id}/orderItems")]
    public async Task<ActionResult<ServiceResponse<List<OrderItemResponse>>>> GetOrderItemByOrderIdAsync([FromRoute] int id)
    {
        ServiceResponse<OrderResponse> order = await _orderService.GetOrderByIdAsync(id);
        if (order.Data == null) return ToProblem(order.Status);
        if (!User.CanAccessUser(order.Data.UserId)) return Forbid();
        return ToResponse(await _orderService.GetOrderItemByOrderIdAsync(id));
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse<OrderResponse>>> CreateOrderAsync([FromBody] CreateOrderRequest createOrderRequest)
    {
        ServiceResponse<OrderResponse> createOrder = await _orderService.CreateOrderAsync(User.GetUserId(), createOrderRequest);
        if (createOrder.Data == null) return ToProblem(createOrder.Status);
        return CreatedAtAction("GetOrderById", new { id = createOrder.Data.Id }, createOrder);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<ServiceResponse<OrderResponse>>> DeleteOrderAsync([FromRoute] int id) =>
        ToResponse(await _orderService.DeleteOrderAsync(id));
}
