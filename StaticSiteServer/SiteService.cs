using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticSiteServer
{
    internal class SiteService : ISiteService
    {
        private readonly ISiteConfiguration _config;
        private readonly ISiteHeaderService _headerService;
        public SiteService(ISiteConfiguration siteConfiguration, ISiteHeaderService staticHeaderService)
        {
            _config = siteConfiguration;
            _headerService = staticHeaderService;
        }
        public Task<IActionResult> Run(HttpRequest req, string? file, string route = "")
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(route))
                {
                    file = $"{route}/{file}";
                }
                var filePath = GetFilePath(file ?? "");
                if (File.Exists(filePath))
                {
                    var stream = File.OpenRead(filePath);
                    _headerService.AddHeadersToRequest(req, filePath);
                    return Task.FromResult<IActionResult>(new FileStreamResult(stream, GetMimeType(filePath))
                    {
                        LastModified = File.GetLastWriteTime(filePath)
                    });
                }
                else
                {
                    return Task.FromResult<IActionResult>(new NotFoundResult());
                }
            }
            catch
            {
                return Task.FromResult<IActionResult>(new BadRequestResult());
            }
        }
        private string GetFilePath(string pathValue)
        {
            string fullPath = Path.GetFullPath(Path.Combine(_config.ContentRoot, pathValue));
            if (!IsInDirectory(_config.ContentRoot, fullPath))
            {
                throw new ArgumentException("Invalid path");
            }
            if (Directory.Exists(fullPath))
            {
                fullPath = Path.Combine(fullPath, _config.DefaultPage);
            }
            return fullPath;
        }

        private static bool IsInDirectory(string parentPath, string childPath) => childPath.StartsWith(parentPath);

        private static string GetMimeType(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            return MimeTypeMap.GetMimeType(fileInfo.Extension);
        }
    }
    public interface ISiteService
    {
        Task<IActionResult> Run(HttpRequest req, string? file, string route = "");
    }
}
