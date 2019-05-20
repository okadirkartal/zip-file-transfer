using System.Threading.Tasks;
using Core.Entities;
using Microsoft.Extensions.Configuration;
using Security.Authenticator.Services.Contracts;
using Security.Cryptor.Contracts;

namespace Security.Authenticator.Services
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

            return _configuration[Core.Common.Constants.CredentialUserName] == userModel.UserName
                   &&
                   _configuration[Core.Common.Constants.CredentialPassword] == userModel.Password
                ? userModel
                : null;
        }
    }
}