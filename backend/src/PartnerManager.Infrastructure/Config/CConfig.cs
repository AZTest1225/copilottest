namespace PartnerManager.Infrastructure.Config
{
    /// <summary>
    /// Centralized configuration values resolved from appsettings and environment variables.
    /// </summary>
    public class CConfig
    {
        public string? DefaultConnection { get; set; }
        public string? JwtKey { get; set; }
        public string? JwtIssuer { get; set; }
        public string? JwtAudience { get; set; }
        public int JwtExpireMinutes { get; set; } = 60;
        public string Environment { get; set; } = string.Empty;
    }
}
