using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticSiteServer
{
    internal class SiteConfiguration : ISiteConfiguration
    {
        public string ContentRoot { get; }

        public string DefaultPage { get; }

        public SiteConfiguration(IConfiguration configuration)
        {
            var localRoot = Environment.GetEnvironmentVariable("AzureWebJobsScriptRoot");
            var azureRoot = $@"{Environment.GetEnvironmentVariable("HOME")}\site\wwwroot";
            ContentRoot = Path.GetFullPath(Path.Combine(
                localRoot is null ? azureRoot : localRoot,
                configuration.GetValue("CONTENT_ROOT", "wwwroot")));
            DefaultPage = configuration.GetValue("DEFAULT_PAGE", "index.html");
        }
    }
    public interface ISiteConfiguration
    {
        string ContentRoot { get; }
        string DefaultPage { get; }
    }
}
