using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService
{
    public class Program
    {
        
        
        public static void Main(string[] args)
        {           
            CreateHostBuilder(args).Build().Run();
        }        

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(builder =>
                {
                    builder.AddJsonConsole(options =>
                    {
                        options.IncludeScopes = false;
                        options.TimestampFormat = "hh:mm:ss ";
                        options.JsonWriterOptions = new System.Text.Json.JsonWriterOptions { Indented = true };
                    });
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
