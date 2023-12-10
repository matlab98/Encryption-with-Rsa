using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace RsaEncryption.WebApi.Entities.Dto;

/// <summary>
/// Este indica el tamaño de la llave 1024|2048|3072|4096
/// </summary>
public class GenerateRequest
{
    /// <summary>
     /// Este indica el tamaño de la llave 1024|2048|3072|4096
    /// </summary>
    [FromHeader]
    [Required(ErrorMessage = "El campo {0} es requerido.")]
    [AllowedValues(1024, 2048, 3072, 4096, ErrorMessage = "El campo {0} debe ser 1024|2048|3072|4096.")]
    public int sizeKey { get; set; }
}
