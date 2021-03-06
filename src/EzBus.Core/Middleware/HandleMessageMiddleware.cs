﻿using System;
using System.Reflection;
using EzBus.Logging;
using EzBus.ObjectFactory;

namespace EzBus.Core.Middleware
{
    internal class HandleMessageMiddleware : ISystemMiddleware
    {
        private static readonly ILogger log = LogManager.GetLogger<HandleMessageMiddleware>();
        private readonly IHandlerCache handlerCache;
        private readonly IObjectFactory objectFactory;
        private readonly IBusConfig busConfig;

        public HandleMessageMiddleware(IHandlerCache handlerCache, IObjectFactory objectFactory, IBusConfig busConfig)
        {
            this.handlerCache = handlerCache ?? throw new ArgumentNullException(nameof(handlerCache));
            this.objectFactory = objectFactory ?? throw new ArgumentNullException(nameof(objectFactory));
            this.busConfig = busConfig ?? throw new ArgumentNullException(nameof(busConfig));
        }

        public void Invoke(MiddlewareContext context, Action next)
        {
            var channelMessage = context.ChannelMessage;
            var messageTypeName = channelMessage.GetHeader(MessageHeaders.MessageFullname);
            var handlerInfos = handlerCache.GetHandlerInfo(messageTypeName);

            foreach (var info in handlerInfos)
            {
                var handlerType = info.HandlerType;

                log.Verbose($"Invoking handler {handlerType.Name}");

                var result = InvokeHandler(handlerType, context.Message);

                if (!result.Success)
                {
                    throw result.Exception;
                }
            }

            next();
        }

        public void OnError(Exception ex)
        {

        }

        private InvokationResult InvokeHandler(Type handlerType, object message)
        {
            var success = true;
            Exception exception = null;

            for (var i = 0; i < busConfig.NumberOfRetries; i++)
            {
                var methodInfo = handlerType.GetMethod("Handle", new[] { message.GetType() });

                try
                {
                    var handler = objectFactory.GetInstance(handlerType);
                    methodInfo.Invoke(handler, new[] { message });
                    success = true;
                    break;
                }
                catch (Exception ex)
                {
                    log.Error(string.Format("Attempt {1}: Failed to handle message '{0}'.", message.GetType().Name, i + 1), ex.InnerException);
                    success = false;
                    exception = ex.InnerException;
                }
            }

            return new InvokationResult(success, exception);
        }
    }
}