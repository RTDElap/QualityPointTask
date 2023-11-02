

using QualityPointTask.Infrastructure.Enums;

namespace QualityPointTask.Infrastructure.Extensions;

public static class InfrastructureExtensions
{
    public static MailingQuality ParseMailingQuality(this string cqComplete)
    {
        int code = int.Parse(cqComplete);

        return code switch
        {
            0 => MailingQuality.Yes,
            10 | 5 | 8 | 9 => MailingQuality.Maybe,
            1 | 2 | 3 | 4 | 5 | 6 | 7 => MailingQuality.No,

            _ => throw new ArgumentOutOfRangeException($"Недопустимое значение: {code}")
        };
    }
}
