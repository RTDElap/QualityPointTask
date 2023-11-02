

namespace QualityPointTask.Infrastructure.Enums;

/// <summary>
/// Пригодность к рассылке
/// </summary>
public enum MailingQuality : byte
{
    // Enum для сопоставления с qc_complete (см. https://dadata.ru/api/clean/address/#qc_complete)

    Yes, // соответствует qc_complete 0 
    Maybe, // соответствует qc_complete 10, 5, 8, 9
    No, // соответствует qc_complete 1, 2, 3, 4, 5, 6, 7
}