using FullTractor.Application.DTOs.Service;

namespace FullTractor.Application.Interfaces;
public interface IOrderService
{
    public Task<ServiceResponse<>> GetAllOrdersAsync();
}