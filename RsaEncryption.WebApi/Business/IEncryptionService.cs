using RsaEncryption.WebApi.Entities;
using RsaEncryption.WebApi.Entities.Dto;

namespace RsaEncryption.WebApi.Business;

public interface IEncryptionService
{
    Task<KgResponse> GenerateKey(GenerateRequest request);
    Task<DefaultResponse> EncryptData(string publicKey, string data);
    Task<DefaultResponse> DecryptData(string privateKey, string data);
}

