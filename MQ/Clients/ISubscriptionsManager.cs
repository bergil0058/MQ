using MQ.Clients.Interfaces;
using MQ.Messages.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQ.Clients
{
    interface ISubscriptionsManager
    {
        IReadOnlyList<IClientSubscription> Subscriptions { get; }
        ISubscription AddSubscription();
        void RemoveSubscription(ISubscription aSubscription);
        List<IClientSubscription> GetSubscriptors(INotification aNotification);
    }
}
