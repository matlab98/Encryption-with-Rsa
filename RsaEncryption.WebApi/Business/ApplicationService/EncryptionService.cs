using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Parameters;
using RsaEncryption.WebApi.Entities;
using RsaEncryption.WebApi.Entities.Config;
using RsaEncryption.WebApi.Entities.Dto;
using RsaEncryption.WebApi.Utilities.Cryptography;
using System.Text;

namespace RsaEncryption.WebApi.Business.ApplicationService;

public class EncryptionService : IEncryptionService
{
    private readonly AppConfig _appConfig;

    public EncryptionService(IOptions<AppConfig> appConfig)
    {
        _appConfig = appConfig.Value;
    }

    public async Task<KgResponse> GenerateKey(GenerateRequest request)
    {        
        EncryptionRsa.GenerateRsaKey(request);

        var publicKeyPem = EncryptionRsa.GetPublicKeyInPemFormat();
        var privateKeyPem = EncryptionRsa.GetPrivateKeyInPemFormat();

        return new KgResponse()
        {
            publicKey = publicKeyPem.Replace("\r\n", ""),
            privateKey = privateKeyPem.Replace("\r\n", "")
        };
    }

    public async Task<DefaultResponse> EncryptData(string publicKey, string data)
    {
        //EncryptionRsa.GenerateRsaKey();
        RsaKeyParameters pk = EncryptionRsa.ConvertPemToRsaKeyParameters(publicKey);

        var encryptedData = EncryptionRsa.EncryptWithPublicKey(pk, JsonConvert.SerializeObject(data));

        return new DefaultResponse()
        {
            statusDescription = "Texto encriptado exitosamente",
            data = new { text = Convert.ToBase64String(encryptedData) }
        };
    }

    public async Task<DefaultResponse> DecryptData(string privateKey, string data)
    {
        //EncryptionRsa.GenerateRsaKey();
        RsaPrivateCrtKeyParameters pk = EncryptionRsa.ConvertPemToRsaPrivateKey(privateKey);
        var decryptedData = EncryptionRsa.DecryptWithPrivateKey(pk, Convert.FromBase64String(data));

        return new DefaultResponse()
        {
            statusDescription = "Texto desencriptado exitosamente",
            data = new { text = Encoding.UTF8.GetString(decryptedData) }
        };
    }

}

