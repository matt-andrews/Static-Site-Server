using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticSiteServer
{
    internal class SiteHeaderService : ISiteHeaderService
    {
        private SiteHeaderFilter[]? _response;
        public void AddResponseHeader(params SiteHeaderFilter[] arr)
        {
            _response = arr;
        }
        public void AddHeadersToResponse(HttpRequest req, string filePath)
        {
            if (_response is null)
                return;
            foreach (var filter in _response)
            {
                if (filePath.EndsWith(filter.Extension))
                {
                    req.HttpContext.Response.Headers.Add(filter.Header, filter.Values);
                }
            }
        }
    }
    public interface ISiteHeaderService
    {
        void AddResponseHeader(params SiteHeaderFilter[] arr);
        void AddHeadersToResponse(HttpRequest req, string filePath);
    }
}
