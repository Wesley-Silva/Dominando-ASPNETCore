using Elmah.Io.AspNetCore;
using Elmah.Io.Extensions.Logging;

namespace ASPNETCoreMVC.Configuration
{
    public static class LoggingConfig
    {
        public static WebApplicationBuilder AddElmahConfiguration(this WebApplicationBuilder builder)
        {
            // Logging - Exception - 404
            builder.Services.Configure<ElmahIoOptions>(builder.Configuration
                .GetSection("ElmahIo"));
            builder.Services.AddElmahIo();

            // Logging + Completo
            builder.Logging.Services.Configure<ElmahIoProviderOptions>(
                builder.Configuration.GetSection("ElmahIo"));
            builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
            builder.Logging.AddElmahIo();

            builder.Logging.AddFilter<ElmahIoLoggerProvider>(null, LogLevel.Warning);

            return builder;
        }
    }
}
