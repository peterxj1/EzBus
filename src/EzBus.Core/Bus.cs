﻿using System;
using EzBus.Core.Routing;
using EzBus.Core.Serilizers;

namespace EzBus.Core
{
    public class Bus : IBus
    {
        private readonly ISendingChannel sendingChannel;
        private readonly IMessageRouting messageRouting;
        private readonly ISubscriptionStorage subscriptionStorage;
        private readonly XmlMessageSerializer serializer;

        public Bus(ISendingChannel sendingChannel, IMessageRouting messageRouting, ISubscriptionStorage subscriptionStorage)
        {
            if (sendingChannel == null) throw new ArgumentNullException("sendingChannel");
            if (messageRouting == null) throw new ArgumentNullException("messageRouting");
            if (subscriptionStorage == null) throw new ArgumentNullException("subscriptionStorage");
            this.sendingChannel = sendingChannel;
            this.messageRouting = messageRouting;
            this.subscriptionStorage = subscriptionStorage;
            serializer = new XmlMessageSerializer();
        }

        public void Send(object message)
        {
            var assemblyName = message.GetType().Assembly.GetName().Name;
            var address = messageRouting.GetRoute(assemblyName, message.GetType().FullName);
            Send(address, message);
        }

        public void Send(string destinationQueue, object message)
        {
            var channelMessage = ChannelMessageFactory.CreateChannelMessage(message, serializer);
            var destination = EndpointAddress.Parse(destinationQueue);
            channelMessage.AddHeader("Destination", destination.ToString());
            sendingChannel.Send(destination, channelMessage);
        }

        public void Publish(object message)
        {
            var endpoints = subscriptionStorage.GetSubscribersEndpoints(message.GetType());

            foreach (var endpoint in endpoints)
            {
                Send(endpoint, message);
            }
        }
    }
}