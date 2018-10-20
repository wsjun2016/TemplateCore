using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using BasisFrameWork.Extension.RabbitMQ.Base;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BasisFrameWork.Extension.RabbitMQ.Services
{
    public class RabbitMQService: RabbitMQBase, IRabbitMQService {

        public RabbitMQService(IOptions<RabbitMQOption> option)
        {
            Option = option?.Value;
        }

        public void WorkOnRabbitMQ(Action<IConnection, IModel> action)
        {
            if (action != null) {
                using (var connection = GetConnection()) {
                    using (var channel = connection.CreateModel()) {
                        action(connection, channel);
                    }
                }
            }
        }
        
        public void Publish(byte[] bytes)
        {
            if (bytes?.Length > 0) {
                WorkOnRabbitMQ((connection, channel) =>
                {
                    channel.ExchangeDeclare("e.defaultOnUser", "direct",true);
                    channel.QueueDeclare("e.defaultOnUser.queue", true, false, false, null);
                    channel.QueueBind("e.defaultOnUser.queue", "e.defaultOnUser", "defaultOnUser");
                    channel.BasicPublish(exchange: "e.defaultOnUser", routingKey: "defaultOnUser", basicProperties: null, body: bytes);
                });
            }
        }

        public void Publish<TEntity>(TEntity entity)
        {
            using (MemoryStream ms = new MemoryStream()) {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, entity);
                Publish(ms.ToArray());
            }
        }

        public void Consumer(Action<object> action)
        {
            if (action != null) {
                WorkOnRabbitMQ((connection, channel) =>
                {
                    channel.QueueDeclare("e.defaultOnUser.queue", true, false, false, null);
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (sender, e) =>
                    {
                        var body = e.Body;
                        if (body?.Length > 0) {
                            using (MemoryStream ms = new MemoryStream(body)) {
                                ms.Position = 0;
                                BinaryFormatter formatter = new BinaryFormatter();
                                var model = formatter.Deserialize(ms);
                                action(model);
                            }
                        }
                    };
                    channel.BasicConsume("e.defaultOnUser.queue", false, consumer);
                });
            }
        }
    }
}
