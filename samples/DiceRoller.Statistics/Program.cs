﻿using System;
using EzBus.RabbitMQ;

namespace DiceRoller.Statistics
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "DiceRoller Statistics";
            Bus.Configure().UseRabbitMQ();
            Bus.Subscribe("diceroller.service");

            Console.Read();
        }
    }
}