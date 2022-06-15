using Core.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using Web.DTO.Core;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [AllowAnonymous]
    public class CoreController : ControllerBase
    {
        private readonly IUser _user;

        public CoreController(IUser user)
        {
            _user = user;
        }

        [HttpPost]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> AddUser([FromBody] UserAddModel userAddModel)
        {
            if (userAddModel is null)
                return BadRequest("Null object");
            UserAddDTQ userAddQuery = new UserAddDTQ();
            userAddQuery.Name = userAddModel.Name;
            userAddQuery.Email = userAddModel.Email;
            userAddQuery.Password = userAddModel.Password;
            bool userAdded = await this._user.AddUser(userAddQuery);

            return Ok(userAdded);
        }
    }
}
