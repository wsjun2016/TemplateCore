using BasisFrameWork.Extension.RabbitMQ.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Demonstration.RabbitMQ
{
    public class RabbitMQDemoService: IRabbitMQDemoService
    {

        private readonly IRabbitMQService _mqService;
        public RabbitMQDemoService(IRabbitMQService mqService)
        {
            _mqService = mqService;
        }

        public void Publish(string value)
        {
            _mqService.Publish(value);
        }
    }
}
