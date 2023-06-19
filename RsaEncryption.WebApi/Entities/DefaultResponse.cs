using System.ComponentModel;

namespace RsaEncryption.WebApi.Entities
{
    public class DefaultResponse
    {
        [DefaultValue("true")]
        public bool status { get; set; }
        public string statusDescription { get; set; }
        public dynamic data { get; set; }
    }
}
