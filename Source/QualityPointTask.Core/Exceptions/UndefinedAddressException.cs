

namespace QualityPointTask.Core.Exceptions;

/// <summary>
/// Существуют альтернативные варианты адреса. Соответствует qc 3 (см. https://dadata.ru/api/clean/address/#qc)
/// </summary>
public class UndefinedAddressException : Exception
{
    public UndefinedAddressException() : base()
    { }

    public UndefinedAddressException(string? message) : base(message)
    { }

    public UndefinedAddressException(string? message, Exception? innerException) : base(message, innerException)
    { }
}