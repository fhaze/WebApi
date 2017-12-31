using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Exceptions;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers
{
    [Route("[controller]")]
    public class RegisterController : Controller
    {
        private readonly AccountService _accountService;

        public RegisterController(AccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        public IActionResult Register([FromBody] AccountRegisterRequest request)
        {
            if (!TryValidateModel(request))
                return BadRequest();
            
            try
            {
                _accountService.Register(request.NickName, request.Email, request.Password);
                return Ok();
            }
            catch (ServiceException e)
            {
                return BadRequest(new {message = e.Message});
            }
        }
    }
}