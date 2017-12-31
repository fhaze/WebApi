using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp.Security;

namespace WebApp.Controllers
{
    [AuthRoles(Role.User)]
    [Route("[controller]")]
    public class InfoController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok(new
            {
                claims = User.Claims.Select(c => new { type = c.Type, value = c.Value })
            });
        }
    }
}