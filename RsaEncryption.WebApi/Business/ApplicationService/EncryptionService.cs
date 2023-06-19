using Newtonsoft.Json;
using RsaEncryption.WebApi.Utilities.Cryptography;
using System.Text;

namespace RsaEncryption.WebApi.Business.ApplicationService
{
    public class EncryptionService : IEncryptionService
    {
        public async Task<dynamic> GenerateKey()
        {
            EncryptionRsa.GenerateRsaKey();

            var publicKeyPem = EncryptionRsa.GetPublicKeyInPemFormat();
            var privateKeyPem = EncryptionRsa.GetPrivateKeyInPemFormat();

            return new
            {
                publicKeyPem = publicKeyPem.Replace("\r\n", ""),
                privateKeyPem = privateKeyPem.Replace("\r\n", "")
            };
        }

        public async Task<dynamic> EncryptData(string data)
        {
            EncryptionRsa.GenerateRsaKey();

            var encryptedData = EncryptionRsa.EncryptWithPublicKey(JsonConvert.SerializeObject(data));

            return new
            {
                encryptedText = Convert.ToBase64String(encryptedData)
            };
        }

        public async Task<dynamic> DecryptData(string data)
        {
            EncryptionRsa.GenerateRsaKey();
            var decryptedData = EncryptionRsa.DecryptWithPrivateKey(Convert.FromBase64String(data));

            return new
            {
                decryptedText = Encoding.UTF8.GetString(decryptedData)
            };
        }

    }
}
