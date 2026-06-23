using FullTractor.Domain.Entities;

namespace FullTractor.Application.Interfaces;
public interface ITokenService
{
    public (string token, DateTime expiryTime) CreateToken(User user);
}