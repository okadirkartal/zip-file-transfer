using Domain.Entities;
using Infrastructure.Security.Contracts;
using Infrastructure.Services.Contracts;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services;

public class UserService : IUserService
{
    private readonly IConfiguration _configuration;

    private readonly IDecrypter _decrypter;

    public UserService(IConfiguration configuration, IDecrypter decrypter)
    {
        _configuration = configuration;
        _decrypter = decrypter;
    }

    public async Task<UserModel?> AuthenticateAsync(string username, string password)
    {
        var user = new UserModel
        {
            UserName = await _decrypter.DecryptAsync(username),
            Password = await _decrypter.DecryptAsync(password)
        };
        return _configuration["Credentials:UserName"] == user.UserName
               &&
               _configuration["Credentials:Password"] == user.Password
            ? user
            : null;
    }
}