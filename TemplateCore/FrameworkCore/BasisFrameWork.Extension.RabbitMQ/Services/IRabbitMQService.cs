using System;
using System.Collections.Generic;
using System.Text;
using BasisFrameWork.Dependency.Interfaces;
using RabbitMQ.Client;

namespace BasisFrameWork.Extension.RabbitMQ.Services
{
    public interface IRabbitMQService: ISingletonService {
        void WorkOnRabbitMQ(Action<IConnection, IModel> action);
        void Publish(byte[] bytes);
        void Publish<TEntity>(TEntity entity);
        void Consumer(Action<object> action);
    }
}
