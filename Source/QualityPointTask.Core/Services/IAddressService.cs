

using QualityPointTask.Infrastructure.Models;

namespace QualityPointTask.Core.Services;

public interface IAddressService
{
    /// <summary>
    /// Возвращает информацию об адресе, исходя из его составляющих
    /// </summary>
    /// <param name="addressParts">Составляющие адреса (например, ["мск", "сухонская", "83/14"])</param>
    /// <returns>Информация об адресе</returns>
    public Task<AddressResult> GetAddressResultFromAsync(string[] addressParts, CancellationToken token);
}