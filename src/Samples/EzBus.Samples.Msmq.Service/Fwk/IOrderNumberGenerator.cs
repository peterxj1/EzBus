﻿using System;

namespace EzBus.Samples.Msmq.Service.Fwk
{
    public interface IOrderNumberGenerator
    {
        int GenerateNumber(Guid orderId);
    }
}