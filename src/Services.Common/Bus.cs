using System;
using ServiceStack;
using ServiceStack.Logging;
using ServiceStack.Messaging;

namespace Services.Common
{
    /*
    export PORT=8082
    export RABBIT_HOST=192.168.99.100
    export RABBIT_PORT=32789
    */   
    public class Env
    {
        public static int Port { get { return int.Parse(Environment.GetEnvironmentVariable("PORT")); } }
        public static string Rabbit_Host { get { return Environment.GetEnvironmentVariable("RABBIT_HOST"); } }
        public static int Rabbit_Port { get { return int.Parse(Environment.GetEnvironmentVariable("RABBIT_PORT")); } }
    }

    public interface IBus
    {
        void Publish<TEvent>(TEvent message);
        TResponse Send<TRequest, TResponse>(TRequest request);
    }

    public class Bus : IBus
    {
        private ILog _log = LogManager.GetLogger(typeof(Bus));
        private readonly IMessageService _messageService;
        
        public Bus(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public void Publish<TEvent>(TEvent message)
        {
            _log.Debug($"Publishing to {QueueNames<TEvent>.In}");
            using (var mqClient = _messageService.CreateMessageQueueClient())
            { 
                mqClient.Publish<TEvent>(message);
            }
        }

        public TResponse Send<TRequest, TResponse>(TRequest request)
        {
            using (var mqClient = _messageService.CreateMessageQueueClient())
            { 
                var queue = mqClient.GetTempQueueName();
            
                _log.Debug($"Sending to {QueueNames<TRequest>.In} and will receive reply on {queue}");
                mqClient.Publish(new Message<TRequest>(request) { ReplyTo = queue });

                _log.Debug($"Waiting for reply.");
                var response = mqClient.Get<TResponse>(queue);
                mqClient.Ack(response);
                
                _log.Debug($"Reply received and acknowledged.");

                return response.GetBody();
            }
        }
    }
}
