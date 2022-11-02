using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Rockstar.Nick
{
  public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
  {
    public BasicAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock
        ) : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
      var authHeader = Request.Headers["Authorization"].ToString();
      if (authHeader != null && authHeader.StartsWith("basic", StringComparison.OrdinalIgnoreCase))
      {
        var token = authHeader.Substring("Basic ".Length).Trim();
        System.Console.WriteLine(token);
        var credentialstring = Encoding.UTF8.GetString(Convert.FromBase64String(token));
        var credentials = credentialstring.Split(':');

        string userName = credentials[0];
        string password = credentials[1];

        if (userName.EndsWith("teamrockstars.nl") && password == "HappyDevelopers")
        {
          var claims = new[] { new Claim("name", credentials[0]), new Claim(ClaimTypes.Role, "Admin") };
          var identity = new ClaimsIdentity(claims, "Basic");
          var claimsPrincipal = new ClaimsPrincipal(identity);
          return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name)));
        }
        throw new Exception("Failed to login");
        Response.StatusCode = 401;
        Response.Headers.Add("WWW-Authenticate", "Basic realm=\"dotnetthoughts.net\"");
        return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
        //else
        //{
        //  throw new Exception("Failed to login");
        //}
      }
      else
      {
        Response.StatusCode = 401;
        Response.Headers.Add("WWW-Authenticate", "Basic realm=\"dotnetthoughts.net\"");
        return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
      }
    }
  }
}
