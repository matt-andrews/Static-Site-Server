using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticSiteServer
{
    public static class Extensions
    {
        public static IServiceCollection AddStaticSiteServing(this IServiceCollection services, Action<ISiteHeaderService> headers)
        {
            var headerService = new SiteHeaderService();
            headers.Invoke(headerService);

            return services
                .AddScoped<ISiteConfiguration, SiteConfiguration>()
                .AddScoped<ISiteService, SiteService>()
                .AddSingleton<ISiteHeaderService>(headerService);
        }
        public static IServiceCollection AddStaticServing(this IServiceCollection services)
        {
            return services.AddStaticSiteServing(o => { });
        }
    }
}
