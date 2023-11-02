using QualityPointTask.Infrastructure.Enums;

namespace QualityPointTask.Infrastructure.Models;

public class AddressResult
{
    public string? Result { get; set; }

    public string? Country { get; set; }

    public string? Region { get; set; }

    public string? Area { get; set; }

    public string? SubArea { get; set; }

    public string? City { get; set; }

    public string? CityDistrict { get; set; }

    public string? Settlement { get; set; }

    public string? Street { get; set; }

    public string? House { get; set; }

    public string? Block { get; set; }

    public MailingQuality? MailingQuality { get; set; } = null;
}