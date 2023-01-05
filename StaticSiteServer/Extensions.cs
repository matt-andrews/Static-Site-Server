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
                .AddSingleton<ISiteConfiguration, SiteConfiguration>()
                .AddSingleton<ISiteService, SiteService>()
                .AddSingleton<ISiteHeaderService>(headerService);
        }
        public static IServiceCollection AddStaticSiteServing(this IServiceCollection services)
        {
            return services.AddStaticSiteServing(o => { });
        }
    }
}
