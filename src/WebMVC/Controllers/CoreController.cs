using Core.Contract;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Web.DTO.Core;
using WebMVC.Models;

namespace WebMVC.Controllers
{
    public class CoreController : Controller
    {
        private readonly IUser _user;

        public CoreController(IUser user)
        {
            _user = user;
        }

        public IActionResult UserView()
        {
            UserAddViewModel userAddViewModel = new UserAddViewModel();
            return View(userAddViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> AddUser(UserAddViewModel userAddViewModel)
        {
            UserAddDTQ userAddQuery = new UserAddDTQ();
            userAddQuery.Name = userAddViewModel.Name;
            userAddQuery.Email = userAddViewModel.Email;
            userAddQuery.Password = userAddViewModel.Password;
            await this._user.AddUser(userAddQuery);
            return View(userAddViewModel);
        }
    }
}