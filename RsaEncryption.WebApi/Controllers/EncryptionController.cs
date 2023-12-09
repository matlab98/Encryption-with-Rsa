using Microsoft.AspNetCore.Mvc;
using RsaEncryption.WebApi.Business;

namespace RsaEncryption.WebApi.Controllers;

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
