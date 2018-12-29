using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TokenAuthorization.Helpers;

namespace TokenAuthorization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProtectedController : ControllerBase
    {
        /// <summary>
        /// Test endpoint
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = Constants.ProtectedUserRole)]
        public ActionResult<string> Get()
        {
            return Ok("Authorized");
        }
    }
}