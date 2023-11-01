using AutoMapper;
using Dadata;
using Dadata.Model;
using Microsoft.Extensions.Logging;
using QualityPointTask.Core.Services;
using QualityPointTask.Infrastructure.Models;

namespace QualityPointTask.Services;

public class AddressService : IAddressService
{
    private readonly ILogger<AddressService> _logger;
    private readonly ICleanClientAsync _cleanClientAsync;
    private readonly IMapper _mapper;

    public AddressService(ILogger<AddressService> logger, ICleanClientAsync cleanClientAsync, IMapper mapper) =>
        (_logger, _cleanClientAsync, _mapper) = (logger, cleanClientAsync, mapper);

    public async Task<AddressResult> GetAddressResultFromAsync(string[] addressParts, CancellationToken token = default)
    {
        string sourceAddress = string.Join(' ', addressParts);

        Address cleanAddress = await _cleanClientAsync.Clean<Address>(sourceAddress);

        return _mapper.Map<AddressResult>(cleanAddress);
    }
}
