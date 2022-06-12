using Core.Contract;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Web.Contract.Identity;
using Web.DTO.Core;
using Web.DTO.Identity;

namespace Web.Service.Identity
{
    public class IdentityAuthentication : IIdentityAuthentication
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly IUser _user;

        public IdentityAuthentication(IHttpContextAccessor httpContext, IUser user)
        {
            _httpContext = httpContext;
            _user = user;
        }

        public async Task<bool> SignIn(IdentityAuthenticateDTQ identityAuthenticateQuery)
        {
            UserGetDTQ userGetQuery = new UserGetDTQ();
            userGetQuery.Email = identityAuthenticateQuery.Email;
            userGetQuery.Password = identityAuthenticateQuery.Password;
            UserDTO user = await this._user.GetUser(userGetQuery);

            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal();
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Email));
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            claims.Add(new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"));
            ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            claimsPrincipal.AddIdentity(identity);
            AuthenticationProperties authenticationProperties = new AuthenticationProperties() { IsPersistent = false, ExpiresUtc = DateTimeOffset.MaxValue };
            await this._httpContext.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authenticationProperties);
            return true;
        }
    }
}