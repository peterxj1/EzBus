﻿using System;
using EzBus;
using EzBus.Core.Resolvers;
using EzBus.ObjectFactory;
using EzBus.Utils;

// ReSharper disable once CheckNamespace
public sealed class Bus
{
    private IObjectFactory objectFactory;
    private IBus bus;
    private static volatile Bus instance;
    private static readonly object syncRoot = new object();

    private Bus() { }

    private static Bus Instance
    {
        get
        {
            if (instance != null) return instance;

            lock (syncRoot)
            {
                if (instance == null)
                {
                    instance = new Bus();
                }
            }

            return instance;
        }
    }

    public static ITransport Configure(Action<IBusConfig> action = null)
    {
        Instance.InitializeObjectFactory();

        var config = Instance.GetConfig();
        action?.Invoke(config);

        var transport = Instance.GetTransport();

        return transport;
    }

    public static void Send(string destination, object message)
    {
        Instance.GetBus().Send(destination, message);
    }

    public static void Publish(object message)
    {
        Instance.GetBus().Publish(message);
    }

    public static void Subscribe(string endpoint)
    {
        var sm = Instance.objectFactory.GetInstance<ISubscriptionManager>();
        sm?.Subscribe(endpoint);
    }

    private void InitializeObjectFactory()
    {
        var objectFactoryType = TypeResolver.GetType<IObjectFactory>();
        objectFactory = (IObjectFactory)objectFactoryType.CreateInstance();
        objectFactory.Initialize();
    }

    private IBusConfig GetConfig()
    {
        var config = objectFactory.GetInstance<IBusConfig>();
        return config;
    }

    private ITransport GetTransport()
    {
        try
        {
            var transport = objectFactory.GetInstance<ITransport>();
            return transport;
        }
        catch (Exception)
        {
            var host = objectFactory.GetInstance<IHost>();
            return new NullTransport(host);
        }
    }

    private IBus GetBus()
    {
        if (bus != null) return bus;

        var t = TypeResolver.GetType<IBus>();

        if (t.IsLocal())
        {
            bus = objectFactory.GetInstance<IBus>();
            return bus;
        }

        bus = t.CreateInstance() as IBus;
        return bus;
    }
}

