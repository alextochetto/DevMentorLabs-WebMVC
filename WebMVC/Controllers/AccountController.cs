using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using WebMVC.Models;

namespace WebMVC.Controllers
{
    [AllowAnonymous]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            return View(loginViewModel);
        }
    }
}
