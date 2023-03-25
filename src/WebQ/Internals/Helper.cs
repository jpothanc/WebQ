using System;
using System.Collections.Generic;
using System.Text;

namespace WebQ.Internals
{
    public class Helper
    {
        public static string UriCombine(string baseUrl, string url)
        {

            if (Uri.TryCreate(new Uri(baseUrl), url, out Uri result))
            {
                return result?.AbsoluteUri;
            }
            throw new InvalidOperationException($"Invalid Url's - {baseUrl}-{url}");
        }
    }
}
