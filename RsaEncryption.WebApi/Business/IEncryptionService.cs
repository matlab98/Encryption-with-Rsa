using RsaEncryption.WebApi.Entities;

namespace RsaEncryption.WebApi.Business
{
    public interface IEncryptionService
    {
        Task<KgResponse> GenerateKey();
        Task<DefaultResponse> EncryptData(string data);
        Task<DefaultResponse> DecryptData(string data);
    }
}
