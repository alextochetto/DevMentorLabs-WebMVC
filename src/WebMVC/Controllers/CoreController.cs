using Core.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            List<string> lines = new List<string>();
            List<string> filePaths = new List<string>();
            await UploadArquivos(files, lines, filePaths);

            List<ProcedimentoDTO> procedimentos = new List<ProcedimentoDTO>();
            ListarProcedimentos(lines, procedimentos);
            List<ProcedimentoSubGrupoHierarquiaDTO> procedimentosSubGrupoHierarquia = CriarHierarquia(procedimentos);

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.
            //return Ok(new { count = files.Count, filePaths });
            return Ok(new { procedimentosSubGrupoHierarquia });
        }

        private static async Task UploadArquivos(List<IFormFile> files, List<string> lines, List<string> filePaths)
        {
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
                    using (StreamReader sr = new StreamReader(filePath, Encoding.UTF8, true))
                    {
                        string line;
                        // Read and display lines from the file until the end of 
                        // the file is reached.
                        int i = 0; // jump line 1
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (i == 0)
                            {
                                i++;
                                continue;
                            }
                            i++;
                            lines.Add(line);
                        }
                    }
                    System.IO.File.Delete(filePath);
                }
            }
        }

        private static void ListarProcedimentos(List<string> lines, List<ProcedimentoDTO> procedimentos)
        {
            ProcedimentoDTO procedimento;
            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i].ToString();
                string[] parts = line.Split(';');
                if (parts.Length == 1)
                    continue;
                procedimento = new ProcedimentoDTO();
                procedimento.Procedimento = parts[0];
                procedimento.SubGrupo = parts[1];
                procedimento.Grupo = parts[1];
                procedimento.Capitulo = parts[2];
                procedimentos.Add(procedimento);
            }

            //PROCEDIMENTO - ROL 2018
            //SUBGRUPO - ROL 2018
            //GRUPO - ROL 2018
            //CAPÍTULO - ROL 2018
            //OD
            //AMB
            //HCO
            //HSO
            //REF
            //PAC
            //DUT
        }

        private static List<ProcedimentoSubGrupoHierarquiaDTO> CriarHierarquia(List<ProcedimentoDTO> procedimentos)
        {
            List<ProcedimentoSubGrupoHierarquiaDTO> procedimentosSubGrupoHierarquia = new List<ProcedimentoSubGrupoHierarquiaDTO>();
            List<string> subGruposDistintos = procedimentos.Select(_ => _.SubGrupo).Distinct().ToList();

            List<ProcedimentoDTO> procedimentosOrdenadosSubGrupo;
            ProcedimentoSubGrupoHierarquiaDTO procedimentoSubGrupoHierarquia;
            foreach (string subGrupo in subGruposDistintos)
            {
                procedimentoSubGrupoHierarquia = new ProcedimentoSubGrupoHierarquiaDTO();
                procedimentoSubGrupoHierarquia.SubGrupo = subGrupo;
                procedimentoSubGrupoHierarquia.Procedimentos = new List<ProcedimentoDTO>();
                procedimentosOrdenadosSubGrupo = procedimentos.Where(_ => _.SubGrupo == subGrupo).ToList();
                foreach (ProcedimentoDTO procedimento in procedimentosOrdenadosSubGrupo)
                {
                    procedimentoSubGrupoHierarquia.Procedimentos.Add(procedimento);
                }
                procedimentosSubGrupoHierarquia.Add(procedimentoSubGrupoHierarquia);
            }

            return procedimentosSubGrupoHierarquia;
        }
    }
}