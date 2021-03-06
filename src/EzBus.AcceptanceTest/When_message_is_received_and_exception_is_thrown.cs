﻿using System;
using EzBus.AcceptanceTest.Specifications;
using EzBus.AcceptanceTest.TestHelpers;
using Xunit;

namespace EzBus.AcceptanceTest
{
    public class When_message_is_received_and_exception_is_thrown : BusSpecificationBase
    {
        public When_message_is_received_and_exception_is_thrown()
        {
            FailingMessageHandler.Reset();
            bus.Send("Moon", new FailingMessage());
        }

        [Then]
        public void Message_should_be_retried_five_times()
        {
            Assert.InRange(FailingMessageHandler.Retries, 1, 5);
        }

        [Then]
        public void Message_should_be_placed_on_error_queue()
        {
            Assert.Equal("testhost.error", FakeMessageChannel.LastSentDestination.Name);
        }
    }

    public class FailingMessageHandler : IHandle<FailingMessage>
    {
        public static int Retries { get; private set; }

        public void Handle(FailingMessage message)
        {
            Retries++;
            throw new Exception("Testing error in handler.");
        }

        public static void Reset()
        {
            Retries = 0;
        }
    }
}