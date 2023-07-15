using Domain.Entities;
using Infrastructure.Security.Contracts;
using Infrastructure.Services.Contracts;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services;

public class UserService : IUserService
{
    private readonly Credentials _credentials;

    private readonly IDecrypter _decrypter;

    public UserService(IOptions<ApplicationOptions> options, IDecrypter decrypter)
    {
        _credentials = options.Value.Credentials;
        _decrypter = decrypter;
    }

    public async Task<IEnumerable<UserModel>> AuthenticateAsync(string username, string password)
    {
        if(username == null || password == null) 
            return Enumerable.Empty<UserModel>();

        var user = new UserModel
        {
            UserName = await _decrypter.DecryptAsync(username),
            Password = await _decrypter.DecryptAsync(password)
        };
        return _credentials.UserName == user.UserName
               &&
              _credentials.Password == user.Password
            ? new List<UserModel>() { user }
            : Enumerable.Empty<UserModel>();
    }
}