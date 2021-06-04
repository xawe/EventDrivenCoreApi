using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PostService.Data;
using PostService.Entities;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace PostService
{
    public class Program
    {        
        public static void Main(string[] args)
        {
            //ListenForIntegrationEvents(GetDatabaseConnectionString(args));
            CreateHostBuilder(args).Build().Run();
            
        }

        private static string GetDatabaseConnectionString(string[] args)
        {
            var providers = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddCommandLine(args)
                .Build()
                .Providers
                .ToList();
            string connString = string.Empty;
            providers[0].TryGet("ConnectionStrings:PostgreSqlConnectionString", out connString);

            return connString;

        }
        private static void ListenForIntegrationEvents(string sqlConncetionString)
        {
            var factory = new ConnectionFactory();
            var connection = factory.CreateConnection();
            {
                var channel = connection.CreateModel();
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, e) =>
                {
                    var contextOptions = new DbContextOptionsBuilder<PostServiceContext>()
                        .UseNpgsql(sqlConncetionString)
                        .Options;
                    var dbContext = new PostServiceContext(contextOptions);

                    var body = e.Body.ToArray();
                    var message = System.Text.Encoding.UTF8.GetString(body);
                    Console.WriteLine("[x] Received {0}", message);
                    var data = Newtonsoft.Json.Linq.JObject.Parse(message);
                    var type = e.RoutingKey;
                    if (type == "user.add")
                    {
                        dbContext.Users.Add(new User()
                        {
                            ID = data.Value<int>("id"),
                            Name = data.Value<string>("name")
                        });
                        dbContext.SaveChanges();
                    }
                    else if (type == "user.update")
                    {
                        var user = dbContext.Users.First(a => a.ID == data.Value<int>("id"));
                        user.Name = data.Value<string>("newname");
                        dbContext.SaveChanges();
                    }
                };

                channel.BasicConsume(queue: "user.postservice",
                        autoAck: true,
                        consumer: consumer);
            }
        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
