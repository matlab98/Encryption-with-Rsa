using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.IO.Pem;
using Org.BouncyCastle.X509;
using RsaEncryption.WebApi.Business;
using System.Security.Cryptography;
using RsaEncryption.WebApi.Utilities.Cryptography;

namespace RsaEncryption.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EncryptionController : ControllerBase
    {
        /// <summary>
        ///instancia de la interfaz para llamar los Request 
        /// </summary>
        private readonly IEncryptionService _encryptionService;

        public EncryptionController(IEncryptionService encryptionService)
        {
            _encryptionService = encryptionService;
        }

        [HttpGet("keys")]
        public async Task<IActionResult> GetKeys()
        {
            var response = await _encryptionService.GenerateKey();

            return Ok(response);
        }

        [HttpPost("encryptData")]
        public async Task<IActionResult> EncryptData([FromBody] string data)
        {
            try
            {
                var response = await _encryptionService.EncryptData(data);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost("decryptData")]
        public async Task<IActionResult> DecryptData([FromBody] string data)
        {
            try
            {
                var response = await _encryptionService.DecryptData(data);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}