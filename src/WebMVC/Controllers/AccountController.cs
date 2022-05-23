using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Web.Contract.Identity;
using Web.DTO.Identity;
using WebMVC.Models;

namespace WebMVC.Controllers
{
    [AllowAnonymous]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly IIdentityAuthentication _identityAuthentication;

        public AccountController(IHttpContextAccessor httpContext, IIdentityAuthentication identityAuthentication)
        {
            _httpContext = httpContext;
            _identityAuthentication = identityAuthentication;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel loginViewModel)
        {
            IdentityAuthenticateDTQ identityAuthenticateQuery = new IdentityAuthenticateDTQ();
            identityAuthenticateQuery.Email = loginViewModel.Email;
            identityAuthenticateQuery.Password = loginViewModel.Password;
            await this._identityAuthentication.SignIn(identityAuthenticateQuery);
            return LocalRedirect("/Home/");
        }

        public async Task<ActionResult> Logout()
        {
            await this._httpContext.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return LocalRedirect("/Home/");
        }
    }
}
