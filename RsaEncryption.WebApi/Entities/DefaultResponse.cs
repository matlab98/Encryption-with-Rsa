namespace RsaEncryption.WebApi.Entities
{
    public class DefaultResponse
    {
        public bool status { get; set; }
        public string statusDescription { get; set; }
        public dynamic data { get; set; }
    }
}
