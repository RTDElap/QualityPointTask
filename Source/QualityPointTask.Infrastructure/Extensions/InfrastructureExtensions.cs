

using QualityPointTask.Infrastructure.Enums;

namespace QualityPointTask.Infrastructure.Extensions;

public static class InfrastructureExtensions
{
    /// <summary>
    /// Парсит код из строки и возвращает MailingQuality
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Код не входит в диапазон 0-10</exception>
    /// <exception cref="FormatException">Строка не является числом</exception>
    /// <exception cref="OverflowException">Строка содержит слишком большое число</exception>
    /// <param name="cqComplete">Код результата</param>
    /// <returns>MailingQuality - Пригодность к рассылке</returns>
    public static MailingQuality ParseMailingQuality(this string cqComplete)
    {
        switch ( int.Parse(cqComplete) )
        {
            case 0:
                return MailingQuality.Yes;

            case 10:
            case 5:
            case 8:
            case 9:
                return MailingQuality.Maybe;

            case 1:
            case 2:
            case 3:
            case 4:
            case 6:
            case 7:
                return MailingQuality.No;

            default:
                throw new ArgumentOutOfRangeException($"Некорректное значение: {cqComplete}");
        }
    }
}
