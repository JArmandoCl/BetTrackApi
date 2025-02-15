using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BetTrackApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VersionController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        [Route("CurrentVersion")]
        public ActionResult<string> GetVersion()
        {
            return GetType().Assembly.GetName().Version.ToString();
        }
    }
}
