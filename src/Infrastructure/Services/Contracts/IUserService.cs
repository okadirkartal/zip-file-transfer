using Domain.Entities;

namespace Infrastructure.Services.Contracts;

public interface IUserService
{
    Task<IEnumerable<UserModel>> AuthenticateAsync(string username, string password);
}