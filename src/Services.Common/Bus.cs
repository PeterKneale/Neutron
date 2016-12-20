using System;
using ServiceStack;
using ServiceStack.Logging;
using ServiceStack.Messaging;

namespace Services.Common
{
    public interface IBus
    {
        void Publish<TEvent>(TEvent message);
        TResponse Send<TRequest, TResponse>(TRequest request);
    }

    public class Bus : IBus
    {
        private ILog _log = LogManager.GetLogger(typeof(Bus));
        private readonly IMessageQueueClient _mqClient;
        private readonly IMessageProducer _mqProducer;
        
        public Bus(IMessageService messageService)
        {
            _mqClient = messageService.CreateMessageQueueClient();
            _mqProducer = messageService.CreateMessageProducer();
        }

        public void Publish<TEvent>(TEvent message)
        {
             _log.Debug($"Publishing to {QueueNames<TEvent>.In}");
            _mqProducer.Publish<TEvent>(message);
        }

        public TResponse Send<TRequest, TResponse>(TRequest request)
        {
            var queue = _mqClient.GetTempQueueName();
            
            _log.Debug($"Sending to {QueueNames<TRequest>.In} and will receive reply on {queue}");
            _mqClient.Publish(new Message<TRequest>(request) { ReplyTo = queue });

            _log.Debug($"Waiting for reply.");
            var response = _mqClient.Get<TResponse>(queue);
            _mqClient.Ack(response);
            
            _log.Debug($"Reply received and acknowledged.");

            return response.GetBody();
        }
    }
}
