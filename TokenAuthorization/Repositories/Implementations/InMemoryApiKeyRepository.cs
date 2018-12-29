using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TokenAuthorization.Models;
using TokenAuthorization.Repositories.Interfaces;

namespace TokenAuthorization.Repositories.Implementations
{
    public class InMemoryApiKeyRepository : IApiKeyRepository
    {
        private List<ApiKeyModel> _keys = new List<ApiKeyModel>();
        
        /// <inheritdoc />
        public async Task<bool> AddKey(string key)
        {
            return await Task.Run(() =>
            {
                var apiKey = new ApiKeyModel
                {
                    Id = Guid.NewGuid().ToString("N"),
                    IsValid = true,
                    Key = key
                };
                _keys.Add(apiKey);
                return true;
            });
            
        }

        /// <inheritdoc />
        public async Task<bool> IsKeyValid(string key)
        {
            return await Task.Run(() =>
            {
                var found = _keys.FirstOrDefault(m => m.Key == key);

                return found?.IsValid ?? false;
            });
        }

        /// <inheritdoc />
        public async Task<bool> InvalidateKey(string key)
        {
            return await Task.Run(() =>
            {
                var found = _keys.FirstOrDefault(m => m.Key == key);
                if (found == null)
                    return false;
                
                found.IsValid = false;

                return true;
            });
        }
    }
}