

using Microsoft.AspNetCore.Mvc;
using QualityPointTask.Core.Services;
using QualityPointTask.Infrastructure.Models;
using QualityPointTask.Services;

[ApiController]
public class RootController : ControllerBase
{
    private readonly ILogger<RootController> _logger;
    private readonly IAddressService _addressService;

    public RootController(ILogger<RootController> logger, IAddressService addressService) =>
        (_logger, _addressService) = (logger, addressService);

    [HttpGet("/{*addressParts}")]
    public async Task<ActionResult<AddressResult>> OnGet(string addressParts)
    {
        var a = addressParts.Split('/');

        return Ok( await _addressService.GetAddressResultFromAsync(a, default) );
    }
}