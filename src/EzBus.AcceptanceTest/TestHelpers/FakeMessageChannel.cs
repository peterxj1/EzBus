﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace EzBus.AcceptanceTest.TestHelpers
{
    public class FakeMessageChannel : ISendingChannel, IReceivingChannel, IPublishingChannel
    {
        private static readonly List<EndpointAddress> sentDestinations = new List<EndpointAddress>();
        private static Action<ChannelMessage> onMessage;

        public void Send(EndpointAddress destination, ChannelMessage channelMessage)
        {
            channelMessage.BodyStream.Seek(0, 0);
            sentDestinations.Add(destination);
            if (destination.Name.EndsWith("error")) return;

            OnMessage(channelMessage);
        }

        public void Initialize(EndpointAddress inputAddress, EndpointAddress errorAddress)
        {

        }

        public Action<ChannelMessage> OnMessage
        {
            get => onMessage; set => onMessage = value;
        }

        public static EndpointAddress LastSentDestination => sentDestinations.LastOrDefault();

        public static bool HasBeenSentToDestination(string destination)
        {
            return sentDestinations.Any(x => x.Name == destination);
        }

        public static IEnumerable<EndpointAddress> GetSentDestinations()
        {
            return sentDestinations;
        }

        public void Publish(ChannelMessage channelMessage)
        {
            throw new NotImplementedException();
        }
    }
}