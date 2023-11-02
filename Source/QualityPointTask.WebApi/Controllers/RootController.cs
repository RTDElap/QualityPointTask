

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using QualityPointTask.Core.Exceptions;
using QualityPointTask.Core.Services;
using QualityPointTask.Infrastructure.Models;
using QualityPointTask.Services;
using System.Net;

namespace QualityPointTask.WebApi.Controllers;

[ApiController]
public class RootController : ControllerBase
{
    private readonly ILogger<RootController> _logger;
    private readonly IAddressService _addressService;

    public RootController(ILogger<RootController> logger, IAddressService addressService) =>
        (_logger, _addressService) = (logger, addressService);
    
    /// <summary>
    /// Возвращает адрес по его составляющим
    /// </summary>
    /// <param name="addressParts">Адрес в формате /arg1/arg2/arg3</param>
    /// <returns></returns>
    [HttpGet("/{*addressParts}")]
    [ProducesResponseType<AddressResult>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status421MisdirectedRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AddressResult>> Index(string? addressParts)
    {
        using ( _logger.BeginScope($"Запрос: {addressParts}") )
        {
            if ( addressParts is null )
            {
                _logger.LogInformation("Пустой запрос");

                return BadRequest( new { Message = "Укажите данные адреса." } );
            }

            // Обрабатывать пустые вхождения (которые образуются, если вид запроса имеет "////"), нет нужды,
            /// поскольку на итоговый результат не влияют
            var fullAddress = addressParts.Split('/');

            try
            {
                var address = await _addressService.GetAddressResultFromAsync(fullAddress, default);

                _logger.LogInformation("Успешно выполнен");

                return Ok( address );
            }
            catch ( Exception ex )
            {
                _logger.LogError("Ошибка при выполнении запроса: {0}", ex.Message);

                return HandleException( ex );
            }
        }
    }

    /// <summary>
    /// Обрабатывает пользовательские и серверные исключения
    /// </summary>
    /// <param name="exception"></param>
    /// <returns></returns>
    private ActionResult<AddressResult> HandleException(Exception exception)
    {
        return exception switch
        {
            ArgumentNullException => StatusCode( StatusCodes.Status421MisdirectedRequest ),

            NotEnoughDataException => NotFound( new { Message = exception.Message } ),

            UndefinedAddressException => NotFound( new { Message = exception.Message } ),

            ArgumentOutOfRangeException => StatusCode( StatusCodes.Status421MisdirectedRequest ),

            HttpRequestException requestException => HandleHttpRequestException( requestException ),

            _ => StatusCode( StatusCodes.Status500InternalServerError )
        };
    }

    /// <summary>
    /// Обрабатывает исключения только от api сервера
    /// </summary>
    /// <param name="exception"></param>
    /// <returns></returns>
    private ActionResult<AddressResult> HandleHttpRequestException(HttpRequestException exception)
    {
        int defaultCode = (int) HttpStatusCode.MisdirectedRequest;

        return exception.StatusCode switch
        {
            HttpStatusCode.Unauthorized => StatusCode( defaultCode, new { Message = "Не указан или указан неверно токен/секрет." } ),

            HttpStatusCode.Forbidden => StatusCode( defaultCode, new { Message = "Неподтверждена почта или отсутствуют деньги на балансе" } ),

            HttpStatusCode.TooManyRequests => StatusCode( defaultCode, new { Message = "Слишком много запросов на одно соединение" } ),

            _ => StatusCode( defaultCode )
        };
    }
}