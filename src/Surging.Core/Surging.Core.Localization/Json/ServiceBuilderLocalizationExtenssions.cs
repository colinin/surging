using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System;

namespace Surging.Core.Localization.Json
{
    public static class ServiceBuilderLocalizationExtenssions
    {
        public static void AddJsonLocalization(this IServiceCollection services)
        {
            services.AddTransient<IStringLocalizer, JsonStringLocalizer>();
            services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
        }

        public static void AddJsonLocalization(this IServiceCollection services, Action<LocalizationOptions> setupAction)
        {
            services.AddLocalization(setupAction);
            services.AddJsonLocalization();
        }
    }
}
