using System;
using Environment;

namespace EzBus.RabbitMQ
{
    [CLSCompliant(false)]
    public class RabbitMQConfig : IRabbitMQConfig
    {
        public int Port { get; set; } = 5672;
        public bool AutomaticRecoveryEnabled { get; set; } = true;
        public ushort RequestedHeartbeat { get; set; } = 5;
        public ushort PrefetchCount { get; set; } = 100;
        public string ExchangeType { get; set; } = RabbitMQ.ExchangeType.Fanout;
        public string UserName { get; set; } = Environment.GetEnvironmentVariable("rabbit_username") ?? "guest";
        public string Password { get; set; } = Environment.GetEnvironmentVariable("rabbit_password") ?? "guest";
        public string HostName { get; set; } = Environment.GetEnvironmentVariable("rabbit_hostname") ?? "localhost";
        public string VirtualHost { get; set; } = "/";
    }
}