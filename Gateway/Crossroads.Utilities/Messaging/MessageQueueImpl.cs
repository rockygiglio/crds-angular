using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;
using Crossroads.Utilities.Messaging.Interfaces;

namespace Crossroads.Utilities.Messaging
{
    public class MessageQueueImpl : IMessageQueue
    {
        private readonly MessageQueue _messageQueue;

        public MessageQueueImpl(MessageQueue messageQueue)
        {
            _messageQueue = messageQueue;
        }
        public void Send(object message, MessageQueueTransactionType type)
        {
            _messageQueue.Send(message, type);
        }

        public MessageQueue CreateQueue(string queueName, QueueAccessMode accessMode, IMessageFormatter formatter = null)
        {
            var queue = new MessageQueue(queueName, accessMode)
            {
                Formatter = formatter ?? new JsonMessageFormatter(),
                MessageReadPropertyFilter = new MessagePropertyFilter
                {
                    ArrivedTime = true,
                    Body = true
                }
            };
            return (queue);
        }

        public bool Exists(string path)
        {
            return MessageQueue.Exists(path);
        }

        public MessageQueue Create(string path)
        {
            return MessageQueue.Create(path);
        }
    }
}
