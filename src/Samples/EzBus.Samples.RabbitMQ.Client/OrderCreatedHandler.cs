﻿using System;
using EzBus.Samples.Messages.Events;

namespace EzBus.Samples.RabbitMQ.Client
{
    public class OrderCreatedHandler : IHandle<OrderCreated>
    {
        public void Handle(OrderCreated message)
        {
            Console.WriteLine($"{DateTime.Now.ToLocalTime()}: Order created: {message.OrderNumber}");
        }
    }
}