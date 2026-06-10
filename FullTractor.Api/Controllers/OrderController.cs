using FullTractor.Application.DTOs;
using FullTractor.Application.DTOs.Service;
using FullTractor.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using FullTractor.Application.Enums;

namespace FullTractor.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet("{userId}/allOrders")]
    public async Task<ActionResult<ServiceResponse<List<OrderResponse>>>> GetAllOrderByUserIdsAsync([FromRoute] int userId)
    {
        ServiceResponse<List<OrderResponse>> ordersByUserId = await _orderService.GetAllOrderByUserIdsAsync(userId);
        if (ordersByUserId.Status == Status.NotFound) return Problem(statusCode: StatusCodes.Status404NotFound, detail: ordersByUserId.Status.ToString());
        return Ok(ordersByUserId);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceResponse<OrderResponse>>> GetOrderByIdAsync([FromRoute] int id)
    {
        ServiceResponse<OrderResponse> orderById = await _orderService.GetOrderByIdAsync(id);
        if (orderById.Status == Status.NotFound) return Problem(statusCode: StatusCodes.Status404NotFound, detail: orderById.Status.ToString());
        return Ok(orderById);
    }
    [HttpGet("{id}/orderItems")]
    public async Task<ActionResult<ServiceResponse<List<OrderItemResponse>>>> GetOrderItemByOrderIdAsync([FromRoute] int id)
    {
        ServiceResponse<List<OrderItemResponse>> orderItems = await _orderService.GetOrderItemByOrderIdAsync(id);
        if (orderItems.Status == Status.NotFound) return Problem(statusCode: StatusCodes.Status404NotFound, detail: orderItems.Status.ToString());
        return Ok(orderItems);
    }
    [HttpPost]
    public async Task<ActionResult<ServiceResponse<OrderResponse>>> CreateOrderAsync([FromBody] CreateOrderRequest createOrderRequest)
    {
        ServiceResponse<OrderResponse> createOrder = await _orderService.CreateOrderAsync(createOrderRequest);
        if(createOrder.Status == Status.CreateOrderError) return Problem(statusCode: StatusCodes.Status409Conflict, detail: createOrder.Status.ToString());
        return Ok(createOrder);
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult<ServiceResponse<OrderResponse>>> DeleteOrderAsync([FromRoute]int id)
    {
        ServiceResponse<OrderResponse> orderDelete = await _orderService.DeleteOrderAsync(id);
        switch (orderDelete.Status)
        {
            case Status.NotFound:
                return Problem(statusCode: StatusCodes.Status404NotFound, detail: orderDelete.Status.ToString());
            case Status.DeleteError:
                return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: orderDelete.Status.ToString());
            default:
                return Ok(orderDelete);
        }
    }
}