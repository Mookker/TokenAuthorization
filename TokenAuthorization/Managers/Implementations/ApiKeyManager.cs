using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TokenAuthorization.Managers.Interfaces;
using TokenAuthorization.Repositories.Interfaces;

namespace TokenAuthorization.Managers.Implementations
{
    public class ApiKeyManager : IApiKeyManager
    {
        private readonly IPasswordHasher<string> _passwordHasher;
        private readonly IApiKeyRepository _apiKeyRepository;
        private const string Password = "SomePasswordThatLooksSecure321";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="passwordHasher"></param>
        /// <param name="apiKeyRepository"></param>
        public ApiKeyManager(IPasswordHasher<string> passwordHasher, IApiKeyRepository apiKeyRepository)
        {
            _passwordHasher = passwordHasher;
            _apiKeyRepository = apiKeyRepository;
        }

        /// <inheritdoc />
        public async Task<string> GenerateKey()
        {
            var guid = Guid.NewGuid().ToString();
            var key = _passwordHasher.HashPassword(guid, Password);
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