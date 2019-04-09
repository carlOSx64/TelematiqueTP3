using System;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Data;
using WebApi.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SetupController : ControllerBase
    {

        private IUserService _userService;
        private IFileService _fileService;
        private IGroupService _groupService;
        private IConnectedUserService _connectedUserService;

        public SetupController(IUserService userService, IGroupService groupService, IConnectedUserService connectedUserService,
                               IFileService fileService)
        {
            _userService = userService;
            _fileService = fileService;
            _groupService = groupService;
            _connectedUserService = connectedUserService;
        }


        // GET api/setup/reset
        // Anonymous
        [HttpGet("reset")]
        public ActionResult<string> reset()
        {
            // Truncate tables users, group, connectedUser
            _connectedUserService.DeleteAll();
            _groupService.DeleteAll();
            _userService.DeleteAll();
            _fileService.DeleteAll();

            _userService.Create("user1", "user1");
            _userService.Create("user2", "user2");
            _userService.Create("user3", "user3");
            _userService.Create("user4", "user4");
            _userService.Create("user5", "user5");

            _groupService.Create("IFT585");
            _groupService.Create("IFT606");

            _fileService.Create("File1.txt", "c2FsdXQgbGEgZ2FuZyBjJ2VzdCBsZSBjb250ZW51IGRlIEZpbGUxLnR4dA==", _groupService.GetAll().First().Id);
            _fileService.Create("File2.txt", "c2FsdXQgbGEgZ2FuZyBjJ2VzdCBsZSBjb250ZW51IGRlIEZpbGUyLnR4dCBhaGFo", _groupService.GetAll().First().Id);

            _fileService.Create("File3.txt", "ZmlsZTMgYXBwYXJ0aWVudCBhdSAyZSBncm91cGU=", _groupService.GetAll()[1].Id);


            // TODO : Reset la séquence pour les IDs de toutes les tables, sinon ça crash
            // Un truc du genre normalement en SQLite
            // UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='Users';
            _groupService.AddUserToGroup(1, 1, true);
            _groupService.AddUserToGroup(2, 1, false);
            _groupService.InviteUser(3, 1, false, 1);
            _groupService.UpdateInvitation(3, 1, InvitationStatus.Rejected);
            _groupService.InviteUser(4, 1, false, 1);
            return Ok();
        }
    }
}