using Core.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Text;
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

        public IActionResult File()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> FileUpload(List<IFormFile> files)
        {
            //long size = files.Sum(f => f.Length);

            List<string> filePaths = new List<string>();
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    // full path to file in temp location
                    var filePath = Path.GetTempFileName(); //we are using Temp file name just for the example. Add your own file path.
                    filePaths.Add(filePath);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    StringBuilder sb = new StringBuilder();
                    using (StreamReader sr = new StreamReader(filePath, Encoding.UTF8, true))
                    {
                        string line;
                        // Read and display lines from the file until the end of 
                        // the file is reached.
                        while ((line = sr.ReadLine()) != null)
                        {
                            sb.AppendLine(line);
                        }
                    }
                    string allines = sb.ToString();
                }
            }
            // process uploaded files
            // Don't rely on or trust the FileName property without validation.
            return Ok(new { count = files.Count, filePaths });
        }
    }
}