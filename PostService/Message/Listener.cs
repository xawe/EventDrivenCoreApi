using PostService.Data;
using PostService.Entities;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace PostService.Message
{
    public class Listener
    {
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
    }
}
