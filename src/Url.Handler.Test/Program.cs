using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Url.Handler.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("url to handle: /media/curioso_el_bicho.jpg");
            Resolve handler = null;
            Console.WriteLine("press any key to continue");
            Console.ReadLine();
            var url = "/media/curioso_el_bicho.jpg";
            try
            {
                handler = new Resolve(url);
                handler.BeforeFilterApply += handler_BeforeFilterApply;
            }
            catch (Exception ex)
            {
                Console.WriteLine("handler can't be loaded for: {0} details: {1}", url, ex.Message);
            }

            handler.AddFilter(absolute);
            Console.WriteLine("filter added: \"converts url to absolute with Host: http://myHost/\"");
            Console.WriteLine("press any key to apply filters and se the result");
            Console.ReadLine();
            Console.WriteLine(string.Format("result : {0}",handler.ApplyFilters()));
            handler.AddFilter(resolveCdnAsset);
            Console.WriteLine("filter added: \"change the asset host to resolve from a CDN\"");
            Console.WriteLine("press any key to apply filters and se the result");
            Console.ReadLine();
            Console.WriteLine(string.Format("result : {0}", handler.ApplyFilters()));
            Console.ReadLine();



        }

        static void handler_BeforeFilterApply(Filter currentFilter, Resolve myArgs)
        {
            Trace.WriteLine("custom event fired - BeforeFilterApply");
        }

        private static string absolute(string url, Resolve holder) {
            
            string host = "http://myHost/";

            if(holder.IsRelative)
                return new Uri(new Uri(host), url).ToString();

            Uri uri = new Uri(url);

            return uri.ToString().Replace(uri.Host, host);
        }

        private static string resolveCdnAsset(string url, Resolve holder)
        {
            string host = "ilauic8723hdqs.cloudfront.net";

            if (holder.IsRelative)
                return new Uri(new Uri(host), url).ToString();

            Uri uri = new Uri(url);

            return uri.ToString().Replace(uri.Host, host);

        }
    }
}
