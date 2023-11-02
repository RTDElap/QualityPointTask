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

    /// <summary>
    /// Возвращает информацию об адресе, исходя из его составляющих
    /// </summary>
    /// <exception cref="ArgumentNullException">CleanClientAsync вернул null, вместо Address</exception>
    /// <exception cref="NotEnoughDataException">Недостаточно данных для поиска</exception>
    /// <exception cref="UndefinedAddressException">Несколько адресов подходят к описанию</exception>
    /// <exception cref="ArgumentOutOfRangeException">Сервер вернул cq_complete в неправильном диапазоне</exception>
    /// <exception cref="HttpRequestException">Ошибочный запрос к api-серверу (см. https://dadata.ru/api/clean/address/#return)</exception>
    /// <param name="addressParts">Составляющие адреса (например, ["мск", "сухонская", "83/14"])</param>
    /// <returns>Информация об адресе</returns>
    public async Task<AddressResult> GetAddressResultFromAsync(string[] addressParts, CancellationToken token = default)
    {
        // Адрес для поиска
        string sourceAddress = string.Join(' ', addressParts);

        Address? cleanAddress = await _cleanClientAsync.Clean<Address>(sourceAddress);

        if ( cleanAddress is null )
            throw new ArgumentNullException($"CleanAddress is null");

        ThrowExceptionIfQualityCodeIsError( cleanAddress.qc );

        AddressResult mappedResult = _mapper.Map<AddressResult>(cleanAddress);

        mappedResult.MailingQuality = cleanAddress.qc_complete.ParseMailingQuality();

        return mappedResult;
    }

    private void ThrowExceptionIfQualityCodeIsError(string qualityCode)
    {
        switch ( qualityCode )
        {
            case "1":
                throw new NotEnoughDataException("Недостаточно данных для поиска.");
            
            case "3":
                throw new UndefinedAddressException("Неоднозначный адрес.");
        }
    }
}
