using System.Runtime.Serialization;

namespace TokenAuthorization.Models
{
    [DataContract]
    public class DisableKeyModel
    {
        [DataMember]
        public string Key { get; set; }
    }
}