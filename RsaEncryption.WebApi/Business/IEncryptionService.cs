namespace RsaEncryption.WebApi.Business
{
    public interface IEncryptionService
    {
        Task<dynamic> GenerateKey();
        Task<dynamic> EncryptData(string data);
        Task<dynamic> DecryptData(string data);
    }
}
