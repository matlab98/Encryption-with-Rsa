using Newtonsoft.Json;
using RsaEncryption.WebApi.Entities;
using RsaEncryption.WebApi.Utilities.Cryptography;
using System.Text;

namespace RsaEncryption.WebApi.Business.ApplicationService;

public class EncryptionService : IEncryptionService
{
    public async Task<KgResponse> GenerateKey()
    {
        EncryptionRsa.GenerateRsaKey();

        var publicKeyPem = EncryptionRsa.GetPublicKeyInPemFormat();
        var privateKeyPem = EncryptionRsa.GetPrivateKeyInPemFormat();

        return new KgResponse()
        {
            publicKey = publicKeyPem.Replace("\r\n", ""),
            privateKey = privateKeyPem.Replace("\r\n", "")
        };
    }

    public async Task<DefaultResponse> EncryptData(string data)
    {
        EncryptionRsa.GenerateRsaKey();

        var encryptedData = EncryptionRsa.EncryptWithPublicKey(JsonConvert.SerializeObject(data));

        return new DefaultResponse()
        {
            statusDescription = "Texto encriptado exitosamente",
            data = new { text = Convert.ToBase64String(encryptedData) }
        };
    }

    public async Task<DefaultResponse> DecryptData(string data)
    {
        EncryptionRsa.GenerateRsaKey();
        var decryptedData = EncryptionRsa.DecryptWithPrivateKey(Convert.FromBase64String(data));

        return new DefaultResponse()
        {
            statusDescription = "Texto desencriptado exitosamente",
            data = new { text = Encoding.UTF8.GetString(decryptedData) }
        };
    }

}

