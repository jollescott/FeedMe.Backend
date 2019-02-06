using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Ramsey.NET.Crawlers.Misc
{
    public class HemmetsHttpClient : HttpClient
    {
        public HemmetsHttpClient()
        {
            DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:64.0) Gecko/20100101 Firefox/64.0");
            DefaultRequestHeaders.Add("Accept-Language", "sv-SE,sv;q=0.8,en-US;q=0.5,en;q=0.3");
            DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            DefaultRequestHeaders.Add("Referer", "https://kokboken.ikv.uu.se/sok.php");
            //_client.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");
            DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
        }
    }
}
