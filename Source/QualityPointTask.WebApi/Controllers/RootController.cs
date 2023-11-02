

using Microsoft.AspNetCore.Mvc;
using QualityPointTask.Core.Exceptions;
using QualityPointTask.Core.Services;
using QualityPointTask.Infrastructure.Models;
using QualityPointTask.Services;

namespace QualityPointTask.WebApi.Controllers;

[ApiController]
public class RootController : ControllerBase
{
    private readonly ILogger<RootController> _logger;
    private readonly IAddressService _addressService;

    public RootController(ILogger<RootController> logger, IAddressService addressService) =>
        (_logger, _addressService) = (logger, addressService);

    [HttpGet("/{*addressParts}")]
    public async Task<ActionResult<AddressResult>> OnGet(string? addressParts)
    {
        if ( addressParts is null )
        {
            return BadRequest( new { Message = "Укажите данные адреса." } );
        }

        var fullAddress = addressParts.Split('/');

        try
        {
            var address = await _addressService.GetAddressResultFromAsync(fullAddress, default);
        
            return Ok( address );
        }
        catch ( NotEnoughDataException ex )
        {
            return NotFound( new { Message = ex.Message } );
        }
        catch ( UndefinedAddressException ex )
        {
            return NotFound( new { Message = ex.Message } );
        }
        catch ( HttpRequestException ex )
        {
            return BadRequest();
        }
    }
}