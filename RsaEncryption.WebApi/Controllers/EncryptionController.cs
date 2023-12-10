using Microsoft.AspNetCore.Mvc;
using RsaEncryption.WebApi.Business;
using RsaEncryption.WebApi.Entities.Dto;

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

    /// <summary>
    /// Endpoint encargado de generar llaves
    /// </summary>
    /// <param name="request"> Request par�metro de header para obtener el tama�o de las llaves que se van a crear </param>
    /// <remarks></remarks>
    /// <response code="200">Consulta realizada con �xito</response>
    /// <response code="400">Petici�n erronea</response>
    /// <response code="500">Problemas en la ejecuci�n</response>
    [HttpGet("keys")]
    public async Task<IActionResult> GetKeys([FromHeader] GenerateRequest request)
    {
        var response = await _encryptionService.GenerateKey(request);

        return Ok(response);
    }

    /// <summary>
    /// Endpoint encargado de recibir y responder a las peticiones de encriptaci�n
    /// </summary>
    /// <param name="request"> Request par�metro para encriptar texto con rsa. </param>
    /// <remarks></remarks>
    /// <response code="200">Consulta realizada con �xito</response>
    /// <response code="400">Petici�n erronea</response>
    /// <response code="500">Problemas en la ejecuci�n</response>
    [HttpPost("encryptData")]
    public async Task<IActionResult> EncryptData([FromHeader] string publicKey, [FromBody] string data)
    {
        try
        {
            var response = await _encryptionService.EncryptData(publicKey, data);

            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    /// <summary>
    /// Endpoint encargado de recibir y responder a las peticiones de consulta de los datos
    /// </summary>
    /// <param name="request"> Request par�metro para desencriptar texto con rsa. </param>
    /// <remarks></remarks>
    /// <response code="200">Consulta realizada con �xito</response>
    /// <response code="400">Petici�n erronea</response>
    /// <response code="500">Problemas en la ejecuci�n</response>
    [HttpPost("decryptData")]
    public async Task<IActionResult> DecryptData([FromHeader] string privateKey, [FromBody] string data)
    {
        try
        {
            var response = await _encryptionService.DecryptData(privateKey, data);

            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
