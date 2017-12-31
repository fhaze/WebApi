using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using WebApp.Security;
using WebApp.Services;

namespace WebApp.Controllers
{
    [AuthRoles(Role.User)]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly AccountService _accountService;
        
        public UserController(AccountService accountService)
        {
            _accountService = accountService;
        }
        
        [AuthRoles(Role.Admin)]
        [HttpGet]
        public IActionResult List()
        {
            return Ok(_accountService.List);
        }
        
        [AuthRoles(Role.Admin)]
        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            return Ok(_accountService.Get(id));
        }
        
        [HttpGet("Me")]
        public IActionResult Me()
        {
            return Ok(_accountService.Current(User));
        }
    }
}