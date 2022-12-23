using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticSiteServer
{
    public class SiteHeaderFilter
    {
        public string Extension { get; }
        public string Header { get; }
        public StringValues Values { get; }
        public SiteHeaderFilter(string extension, string header, StringValues values)
        {
            Extension = extension;
            Header = header;
            Values = values;
        }
    }
}
