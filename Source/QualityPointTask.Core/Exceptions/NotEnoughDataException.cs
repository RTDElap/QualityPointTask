

namespace QualityPointTask.Core.Exceptions;

/// <summary>
/// Не хватает данных для разбора или есть лишние части. Соответствует qc 1 (см. https://dadata.ru/api/clean/address/#qc)
/// </summary>
public class NotEnoughDataException : Exception
{
    public NotEnoughDataException() : base()
    { }

    public NotEnoughDataException(string? message) : base(message)
    { }

    public NotEnoughDataException(string? message, Exception? innerException) : base(message, innerException)
    { }
}