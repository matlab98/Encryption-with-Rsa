

using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.IO.Pem;
using Org.BouncyCastle.X509;
using System.Text;

namespace RsaEncryption.WebApi.Utilities.Cryptography
{
    public class EncryptionRsa
    {
        private static AsymmetricCipherKeyPair keyPair;

        public static void GenerateRsaKey()
        {
            if (keyPair == null)
            {
                GenerateKeyPair();
            }
        }

        private static void GenerateKeyPair()
        {
            var keyGenerator = GeneratorUtilities.GetKeyPairGenerator("RSA");
            keyGenerator.Init(new KeyGenerationParameters(new SecureRandom(), 4096));
            keyPair = keyGenerator.GenerateKeyPair();
        }

        public static string GetPrivateKeyInPemFormat()
        {
            RsaKeyParameters privateKey = (RsaPrivateCrtKeyParameters)keyPair.Private;

            PrivateKeyInfo privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(privateKey);
            var pemObject = new PemObject("PRIVATE KEY", privateKeyInfo.ToAsn1Object().GetEncoded());
            using (var stringWriter = new StringWriter())
            {
                var pemWriter = new PemWriter(stringWriter);
                pemWriter.WriteObject(pemObject);
                pemWriter.Writer.Flush();
                return stringWriter.ToString();
            }
        }

        public static string GetPublicKeyInPemFormat()
        {
            RsaKeyParameters publicKey = (RsaKeyParameters)keyPair.Public;
            SubjectPublicKeyInfo publicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(publicKey);
            var pemObject = new PemObject("PUBLIC KEY", publicKeyInfo.ToAsn1Object().GetEncoded());
            using (var stringWriter = new StringWriter())
            {
                var pemWriter = new PemWriter(stringWriter);
                pemWriter.WriteObject(pemObject);
                pemWriter.Writer.Flush();
                return stringWriter.ToString();
            }
        }

        public static byte[] EncryptWithPublicKey(string plaintext)
        {
            RsaKeyParameters publicKey = (RsaKeyParameters)keyPair.Public;

            byte[] plaintextBytes = Encoding.UTF8.GetBytes(plaintext);

            var cipher = CipherUtilities.GetCipher("RSA/ECB/PKCS1Padding");
            cipher.Init(true, publicKey);
            byte[] encryptedBytes = cipher.DoFinal(plaintextBytes);

            return encryptedBytes;
        }

        public static byte[] DecryptWithPrivateKey(byte[] encryptedBytes)
        {
            RsaKeyParameters privateKey = (RsaPrivateCrtKeyParameters)keyPair.Private;

            var cipher = CipherUtilities.GetCipher("RSA/ECB/PKCS1Padding");
            cipher.Init(false, privateKey);
            byte[] decryptedBytes = cipher.DoFinal(encryptedBytes);

            return decryptedBytes;
        }
    }
}
