using Domain.Entities;

namespace Infrastructure.Services.Contracts;

public interface IUserService
{
    Task<UserModel?> AuthenticateAsync(string username, string password);
}