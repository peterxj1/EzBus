﻿using System;
using EzBus.RabbitMQ;
using EzBus;

namespace DiceRoller.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "DiceRoller Client";

            Bus.Configure().UseRabbitMQ(x =>
            {
                x.Uri = "BROKR";
            });

            var keyInfo = new ConsoleKeyInfo();

            while (keyInfo.Key != ConsoleKey.Escape)
            {
                Console.WriteLine("Press <enter>");
                keyInfo = Console.ReadKey();
                Bus.Send("DiceRoller.Service", new RollTheDice { Attempts = 10000 });
            }
        }
    }
}