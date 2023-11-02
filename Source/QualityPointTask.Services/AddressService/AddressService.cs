using AutoMapper;
using Dadata;
using Dadata.Model;
using Microsoft.Extensions.Logging;
using QualityPointTask.Core.Exceptions;
using QualityPointTask.Core.Services;
using QualityPointTask.Infrastructure.Models;
using QualityPointTask.Infrastructure.Extensions;

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
        // Адрес для поиска
        string sourceAddress = string.Join(' ', addressParts);

        Address cleanAddress = await _cleanClientAsync.Clean<Address>(sourceAddress);

        switch ( cleanAddress.qc )
        {
            case "1":
                throw new NotEnoughDataException();
            
            case "3":
                throw new UndefinedAddressException();
        }

        AddressResult mappedResult = _mapper.Map<AddressResult>(cleanAddress);

        mappedResult.MailingQuality = cleanAddress.qc_complete.ParseMailingQuality();

        return mappedResult;
    }
}
