using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Web.Contract.Identity;
using Web.DTO.Identity;

namespace Web.Service.Identity
{
    public class IdentityAuthentication : IIdentityAuthentication
    {
        private readonly IHttpContextAccessor _httpContext;

        public IdentityAuthentication(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public async Task<bool> SignIn(IdentityAuthenticateDTQ identityAuthenticateQuery)
        {
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal();
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, identityAuthenticateQuery.Email));
            claims.Add(new Claim(ClaimTypes.Email, identityAuthenticateQuery.Email));
            claims.Add(new Claim(ClaimTypes.Name, "Alex Tochetto"));
            ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            claimsPrincipal.AddIdentity(identity);
            AuthenticationProperties authenticationProperties = new AuthenticationProperties() { IsPersistent = false, ExpiresUtc = DateTimeOffset.MaxValue };
            await this._httpContext.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authenticationProperties);
            return true;
        }
    }
}