using FullTractor.Application.Enums;
namespace FullTractor.Application.DTOs.Service;

public class ServiceResponse<T> where T: class
{
    public required Status Status { get; set; }
    public T? Data { get; set; }
}