using MQ.Clients.Interfaces;
using MQ.Exceptions;
using MQ.Messages.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQ.Clients
{
    class SubscriptionsManager : ISubscriptionsManager
    {
        public IReadOnlyList<IClientSubscription> Subscriptions { get; }
        private List<IClientSubscription> InternalSubscriptions { get; } = new List<IClientSubscription>();

        public SubscriptionsManager()
        {
            this.Subscriptions = this.InternalSubscriptions.AsReadOnly();
        }

        public ISubscription AddSubscription()
        {
            Subscription iSubscription = new Subscription()
            {
                Enabled = true
            };

            this.InternalSubscriptions.Add(iSubscription);
            return iSubscription;
        }

        public void RemoveSubscription(ISubscription aSubscription)
        {
            _ = this.InternalSubscriptions.RemoveAll(x => x.SubscriptionId == aSubscription.SubscriptionId);
        }

        public List<IClientSubscription> GetSubscriptors(INotification aNotification)
        {
            return this.InternalSubscriptions
                .Where(x => x.NotificationSubscriptions.Contains(aNotification.GetType().FullName))
                .ToList();
        }
    }
}
