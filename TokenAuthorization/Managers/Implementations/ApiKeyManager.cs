using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using TokenAuthorization.Managers.Interfaces;
using TokenAuthorization.Models;
using TokenAuthorization.Repositories.Interfaces;

namespace TokenAuthorization.Managers.Implementations
{
    public class ApiKeyManager : IApiKeyManager
    {
        private readonly IPasswordHasher<string> _passwordHasher;
        private readonly IApiKeyRepository _apiKeyRepository;
        private readonly IOptions<ApiKeyManagerOptions> _apiKeyManagerOptions;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="passwordHasher"></param>
        /// <param name="apiKeyRepository"></param>
        /// <param name="apiKeyManagerOptions"></param>
        public ApiKeyManager(IPasswordHasher<string> passwordHasher, 
            IApiKeyRepository apiKeyRepository,
            IOptions<ApiKeyManagerOptions> apiKeyManagerOptions)
        {
            _passwordHasher = passwordHasher;
            _apiKeyRepository = apiKeyRepository;
            _apiKeyManagerOptions = apiKeyManagerOptions;
        }

        /// <inheritdoc />
        public async Task<string> GenerateKey()
        {
            var guid = Guid.NewGuid().ToString();
            var key = _passwordHasher.HashPassword(guid, _apiKeyManagerOptions.Value.Password);
            var success = await _apiKeyRepository.AddKey(key);
            if (!success)
                return null;
            
            return key;
        }

        /// <inheritdoc />
        public async Task<bool> IsKeyValid(string key)
        {
            return await _apiKeyRepository.IsKeyValid(key);
        }

        /// <inheritdoc />
        public async Task<bool> InvalidateKey(string key)
        {
            return await _apiKeyRepository.InvalidateKey(key);
        }
    }
}