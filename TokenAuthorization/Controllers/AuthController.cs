using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TokenAuthorization.Helpers;
using TokenAuthorization.Managers.Interfaces;
using TokenAuthorization.Models;

namespace TokenAuthorization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IApiKeyManager _apiKeyManager;
        private readonly IOptions<JwtOptions> _jwtOptions;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="apiKeyManager"></param>
        /// <param name="jwtOptions"></param>
        public AuthController(IApiKeyManager apiKeyManager, IOptions<JwtOptions> jwtOptions)
        {
            _apiKeyManager = apiKeyManager;
            _jwtOptions = jwtOptions;
        }

        /// <summary>
        /// Generates api key for user.
        /// In real project requires Authorization to be validated, but this one
        /// will give keys to everyone
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api-key")]
        public async Task<ActionResult<string>> GenerateApiKey()
        {
            var key = await _apiKeyManager.GenerateKey();
            
            return Ok(key);
        }

        /// <summary>
        /// Marks key as invalid
        /// </summary>
        /// <param name="disableKeyModel"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api-key")]
        public async Task<ActionResult> InvalidateApiKey(DisableKeyModel disableKeyModel)
        {
            var valid = await _apiKeyManager.IsKeyValid(disableKeyModel.Key);
            if (!valid)
                return BadRequest("Key is not valid");

            var success = await _apiKeyManager.InvalidateKey(disableKeyModel.Key);
            if (!success)
                BadRequest("Unable to mark key as invalid");

            return Ok();
        }

        /// <summary>
        /// Generates short-time JWT token
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("jwt")]
        public async Task<ActionResult<string>> GenerateJwtToken()
        {
            if (!Request.Headers.ContainsKey(Constants.ApiKeyHeaderName))
                return Unauthorized($"{Constants.ApiKeyHeaderName} is missing");

            var apiKey = Request.Headers[Constants.ApiKeyHeaderName];
            var isKeyValid = await _apiKeyManager.IsKeyValid(apiKey);
            if (!isKeyValid)
                return Unauthorized("Key is invalid");
            var jwt = JwtGenerator.GenerateProtectedToken(_jwtOptions.Value.Key, _jwtOptions.Value.Iss);
            
            return Ok(jwt);
        }
    }
}