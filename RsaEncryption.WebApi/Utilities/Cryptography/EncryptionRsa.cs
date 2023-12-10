

using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.IO.Pem;
using Org.BouncyCastle.X509;
using RsaEncryption.WebApi.Entities.Dto;
using System.Text;

namespace RsaEncryption.WebApi.Utilities.Cryptography;

public class EncryptionRsa
{
    private static AsymmetricCipherKeyPair keyPair;

    public static void GenerateRsaKey(GenerateRequest request)
    {
        if (keyPair == null)
        {
            GenerateKeyPair(request);
        }
    }

    private static void GenerateKeyPair(GenerateRequest req)
    {
        var keyGenerator = GeneratorUtilities.GetKeyPairGenerator("RSA");
        keyGenerator.Init(new KeyGenerationParameters(new SecureRandom(), req.sizeKey));
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

    public static RsaPrivateCrtKeyParameters ConvertPemToRsaPrivateKey(string pemPrivateKey)
    {
        StringReader stringReader = new StringReader(pemPrivateKey);
        PemReader pemReader = new PemReader(stringReader);

        PemObject pemObject = pemReader.ReadPemObject();

        if (!pemObject.Type.Equals("PRIVATE KEY", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("La cadena proporcionada no es una clave privada en formato PEM.");

        AsymmetricKeyParameter privateKeyParam = PrivateKeyFactory.CreateKey(pemObject.Content);
        RsaPrivateCrtKeyParameters rsaPrivateKeyParameters = privateKeyParam as RsaPrivateCrtKeyParameters;

        if (rsaPrivateKeyParameters == null)
            throw new InvalidOperationException("La clave proporcionada no es un RsaPrivateCrtKeyParameters.");

        return rsaPrivateKeyParameters;
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

    public static RsaKeyParameters ConvertPemToRsaKeyParameters(string pemPublicKey)
    {
        StringReader stringReader = new StringReader(pemPublicKey);
        PemReader pemReader = new PemReader(stringReader);

        PemObject pemObject = pemReader.ReadPemObject();

        if (!pemObject.Type.Equals("PUBLIC KEY", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("La cadena proporcionada no es una clave pública en formato PEM.");

        AsymmetricKeyParameter publicKeyParam = PublicKeyFactory.CreateKey(pemObject.Content);
        RsaKeyParameters rsaKeyParameters = publicKeyParam as RsaKeyParameters;

        if (rsaKeyParameters == null)
            throw new InvalidOperationException("La clave proporcionada no es un RsaKeyParameters.");

        return rsaKeyParameters;
    }

    public static byte[] EncryptWithPublicKey(RsaKeyParameters publicKey, string plaintext)
    {
        //RsaKeyParameters publicKey = (RsaKeyParameters)keyPair.Public;

        byte[] plaintextBytes = Encoding.UTF8.GetBytes(plaintext);

        var cipher = CipherUtilities.GetCipher("RSA/ECB/PKCS1Padding");
        cipher.Init(true, publicKey);
        byte[] encryptedBytes = cipher.DoFinal(plaintextBytes);

        return encryptedBytes;
    }

    public static byte[] DecryptWithPrivateKey(RsaPrivateCrtKeyParameters privateKey, byte[] encryptedBytes)
    {
        //RsaKeyParameters privateKey = (RsaPrivateCrtKeyParameters)keyPair.Private;

        var cipher = CipherUtilities.GetCipher("RSA/ECB/PKCS1Padding");
        cipher.Init(false, privateKey);
        byte[] decryptedBytes = cipher.DoFinal(encryptedBytes);

        return decryptedBytes;
    }
}

