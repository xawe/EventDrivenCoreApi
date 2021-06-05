using PostService.Data;
using PostService.Entities;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace PostService.Message
{
    public class Listener : IListener
    {
        private readonly Data.IUserData _userData;
        public Listener(Data.IUserData userData)
        {
            _userData = userData;
        }
        public void ListenForIntegrationEvents(string sqlConncetionString)
        {
            var factory = new ConnectionFactory();
            var connection = factory.CreateConnection();
            {
                var channel = connection.CreateModel();
                var consumer = new EventingBasicConsumer(channel);
                
                consumer.Received += (model, e) =>
                {
                    var dbContext = this.BuildContext(sqlConncetionString);
                    _userData.dbContext = dbContext;
                    var body = e.Body.ToArray();
                    var message = System.Text.Encoding.UTF8.GetString(body);
                    Console.WriteLine("[x] Received {0}", message);
                    var data = Newtonsoft.Json.Linq.JObject.Parse(message);
                    var type = e.RoutingKey;
                    if (type == "user.add")
                    {
                        _userData.AddUser(BuildUser(data));
                    }
                    else if (type == "user.update")
                    {
                        _userData.UpdateUser(this.BuildUser(data, true));                        
                    }
                    _userData.dbContext.Dispose();
                };

                channel.BasicConsume(queue: "user.postservice",
                        autoAck: true,
                        consumer: consumer);
            }
        }

        private User BuildUser(Newtonsoft.Json.Linq.JObject data, bool isUpdate = false)
        {
            var user = new User();
            user.ID = data.Value<int>("id");
            if (isUpdate)
            {
                user.Name = data.Value<string>("newname");
            }
            else
            {
                user.Name = data.Value<string>("name");
            }

            return user;
        }

        private PostServiceContext BuildContext(string sqlConncetionString)
        {
            var contextOptions = new DbContextOptionsBuilder<PostServiceContext>()
                        .UseNpgsql(sqlConncetionString)
                        .Options;
            var dbContext = new PostServiceContext(contextOptions);
            return dbContext;
        }
    }
}
