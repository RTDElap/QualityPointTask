#nullable disable

namespace QualityPointTask.WebApi.Configs;

/// <summary>
/// Конфиг подключения к Dadata
/// </summary>
public class DadataConfig
{
    public string Token { get; set; }
    
    public string Secret { get; set; }
}