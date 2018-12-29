namespace TokenAuthorization.Models
{
    public class ApiKeyModel
    {
        /// <summary>
        /// Internal id
        /// </summary>
        public string Id { get; set; }
        
        /// <summary>
        /// Key itself
        /// </summary>
        public string Key { get; set; }
        
        /// <summary>
        /// Is valid status
        /// </summary>
        public bool IsValid { get; set; }
    }
}