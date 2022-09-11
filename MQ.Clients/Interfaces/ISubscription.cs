using MQ.Messages.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MQ.Clients.Interfaces
{

    /// <summary>
    /// Contract that will return to client/consumer after subscription
    /// </summary>
    public interface ISubscription
    {
        IEnumerable<string> NotificationSubscriptions { get; }
        IEnumerable<string> RequestResponsesSubscriptions { get; }

        Guid SubscriptionId { get; }

        ISubscription SubscribeTo<T>(Action<T> aCallBack) where T : INotification;
        ISubscription SubscribeTo<T>(Action<T> aCallBack, Predicate<T> aPredicate) where T : INotification;
        ISubscription UnsubscribeFrom<T>() where T : INotification;
        ISubscription SubscribeTo<TIn, TOut>(Func<TIn, TOut> aCallBack) 
            where TIn : IRequest 
            where TOut : IResponse;
        ISubscription UnsubscribeFrom<TIn, TOut>()
            where TIn : IRequest 
            where TOut : IResponse;

        /// <summary>
        /// True to enable subscription and receive messages and/or notifications;
        /// False to disable subscription and stop receiving messages and/or notifications.
        /// </summary>
        bool Enabled { get; set; }

    }
}
