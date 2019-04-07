using Microsoft.AspNetCore.Mvc;
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
        // Anonymous
        [HttpPost]
        public ActionResult<string> Create([FromForm]File fileParam)
        {
            File newFile = this.fileService.Create(fileParam.Name, fileParam.Content);
            return Ok(newFile);
        }

        // POST api/files
        // Anonymous
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