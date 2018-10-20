using System;
using System.Collections.Generic;
using System.Text;
using BasisFrameWork.Dependency.Interfaces;

namespace Application.Demonstration.RabbitMQ
{
   public interface IRabbitMQDemoService:ITransientService
    {
        void Publish(string value);
    }
}
