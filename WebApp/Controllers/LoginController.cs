using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Exceptions;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers
{
    [Route("[controller]")]
    public class LoginController : Controller
    {
        private readonly AccountService _accountService;
        
        public LoginController(AccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        public IActionResult Login([FromBody] AccountLoginRequest request)
        {
            if (!TryValidateModel(request))
                return BadRequest();
            
            try
            {
                var jwtToken = _accountService.Login(request.Login, request.Password);
                return Ok(new { token = jwtToken });
            }
            catch (ServiceException e)
            {
                return BadRequest(new {message = e.Message});
            }
        }
    }
}