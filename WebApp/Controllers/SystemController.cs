using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Security;

namespace WebApp.Controllers
{
    [AuthRoles(Role.Admin)]
    [Route("[controller]")]
    public class SystemController : Controller
    {
        [HttpGet]
        public IActionResult System()
        {
            return Ok(new
            {
                os = RuntimeInformation.OSDescription,
                framework = RuntimeInformation.FrameworkDescription,
            });
        }   
    }
}