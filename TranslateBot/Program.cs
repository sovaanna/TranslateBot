using System.IO;
using System.Net;
using Microsoft.AspNetCore.Hosting;

namespace TranslateBot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel(opt => opt.Listen(IPAddress.Any, 8443))
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .Build();

            host.Run();
        }
    }
}