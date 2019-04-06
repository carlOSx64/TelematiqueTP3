using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using WebApi.Models;
using System;
using WebApi.Data;
using WebApi.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : Controller
    {

        private IFileService fileService;

        public FilesController(IFileService fileService)
        {
            this.fileService = fileService;
        }

        // POST api/files
        [AllowAnonymous]
        [HttpPost]
        public ActionResult<string> Create([FromForm]File fileParam)
        {
            File newFile = this.fileService.Create(fileParam.Name, fileParam.Content);
            return Ok(newFile);
        }

        // POST api/files
        [AllowAnonymous]
        [HttpDelete("{id}")]
        public ActionResult<string> Delete(string id)
        {
            this.fileService.Delete(id);
            return Ok();
        }

        private FileDto ConverFileToFileDto(File file)
        {
            FileDto fileDto = new FileDto()
            {
                Name = file.Name,
                Content = file.Content
            };

            return fileDto;
        }
    }
}