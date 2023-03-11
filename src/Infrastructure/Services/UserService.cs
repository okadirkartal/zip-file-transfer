
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Infrastructure.Services.Contracts;
using Infrastructure.Security.Contracts;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;

        private readonly IDecrypter _decrypter;

        public UserService(IConfiguration configuration, IDecrypter decrypter)
        {
            _configuration = configuration;
            _decrypter = decrypter;
        }

        public async Task<UserModel> AuthenticateAsync(string username, string password)
        {
            var userModel = new UserModel()
            {
                UserName = await _decrypter.DecryptAsync(username),
                Password = await _decrypter.DecryptAsync(password)
            };

            return _configuration["Credentials:UserName"] == userModel.UserName
                   &&
                   _configuration["Credentials:Password"] == userModel.Password
                ? userModel
                : null;
        }
    }
}