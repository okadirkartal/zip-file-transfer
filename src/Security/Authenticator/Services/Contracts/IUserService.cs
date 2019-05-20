using System.Threading.Tasks;
using Core.Entities;

namespace Security.Authenticator.Services.Contracts
{
    public interface IUserService
    {
        Task<UserModel> AuthenticateAsync(string username, string password);
    }
}