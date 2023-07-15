using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Domain.Entities;
using Infrastructure.Services.Contracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Handlers;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IUserService _userService;

    public BasicAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IUserService userService)
        : base(options, logger, encoder, clock)
    {
        _userService = userService;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
            return AuthenticateResult.Fail("Missing Authorization Header");

        var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);

        var credentials = authHeader.Parameter?.Split(':');
        
        if(credentials?.Length != 2)
            return AuthenticateResult.Fail("Missing Authorization Header");


        IEnumerable<UserModel> users;
        try
        {
            
            string username = credentials[0], password = credentials[1];
            
            users = await _userService.AuthenticateAsync(username, password);
            
            if (users.Any())
            {
                UserModel user = users.First();

                var claims = new[]
                    {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.UserData, user.Password)
                };

                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);

                return await Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(principal, Scheme.Name)));

            }
            return await Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
        }
        catch
        {
            return await Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
        }
    }
}