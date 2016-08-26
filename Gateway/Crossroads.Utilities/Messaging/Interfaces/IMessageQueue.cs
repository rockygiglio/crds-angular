using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Crossroads.Utilities.Messaging.Interfaces
{
    public interface IMessageQueue
    {
        void Send(Object message, MessageQueueTransactionType type);
        MessageQueue CreateQueue(string queueName, QueueAccessMode accessMode, IMessageFormatter formatter = null);
        bool Exists(string path);
        MessageQueue Create(string path);
    }
}
