﻿using System;
using EzBus;
using EzBus.Msmq;
using Msmq.Client.Messages;

namespace Msmq.Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Msmq.Client";
            Bus.Configure().UseMsmq();
            Bus.Subscribe("msmq.service");

            var orderId = Guid.NewGuid();
            Bus.Send("msmq.service", new ConfirmOrder(orderId));

            Console.WriteLine($"Order confirmation requested. {orderId}");

            Bus.Unsubscribe("msmq.service");
            Bus.Stop();

            Console.Read();
        }
    }

    public  class LogOnExitTask : IShutdownTask
    {
        public string Name => "LogOnExitTask";
        public void Run()
        {
            Console.WriteLine("LogOnExitTask is running");
        }
    }
}
