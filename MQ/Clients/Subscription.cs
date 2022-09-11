using MQ.Clients.CallBacks;
using MQ.Clients.Interfaces;
using MQ.Messages.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MQ.Clients
{
    /// <summary>
    /// 
    /// </summary>
    public class Subscription : ISubscription
    {

        #region ISubscription

        public IEnumerable<string> NotificationSubscriptions => this.InternalNotificationSubscriptions.Select(x => x.FullName);
        public IEnumerable<string> RequestResponsesSubscriptions => this.InternalRequestResponsesSubscriptions.Select(x => string.Concat(x.Key, " : ", x.Value));

        public Guid SubscriptionId { get; }

        public bool Enabled { get; set; }

        /// <summary>
        /// Returns after all suscribers have processed the notification
        /// </summary>
        /// <param name="aNotification"></param>
        /// <returns></returns>
        public async Task PublishAsync(INotification aNotification)
        {
            // Get all callbacks and invoke them
            IEnumerable<Task> iCallBackTasks = this.GetCallBackTaskEnum(aNotification);

            await Task.WhenAll(iCallBackTasks);

        }


        /// <summary>
        /// Returns inmediatly without waiting suscribers to receive notification
        /// </summary>
        /// <param name="aNotification"></param>
        public void Publish(INotification aNotification)
        {
            List<Task> iCallBackTasks = this.GetCallBackTaskEnum(aNotification).ToList();

            iCallBackTasks.ForEach(x => this.CallbacksTasks.Add(x));
        }

        public ISubscription SubscribeTo<T>(Action<T> aCallBack) where T : INotification
        {
            if (aCallBack is null) throw new ArgumentNullException(nameof(aCallBack));

            Type iType = typeof(T);
            if (!this.InternalNotificationSubscriptions.Any(x => x == iType))
                this.InternalNotificationSubscriptions.Add(iType);

            NotificationCallback iNotifCallBack = NotificationCallback.Create(aCallBack);

            // Remove previous association if any
            //_ = this.NotificationCallbacks.RemoveAll(x => x.NotificationType == iNotifCallBack.NotificationType);

            // Add new one
            this.NotificationCallbacks.Add(iNotifCallBack);

            return this;
        }
        public ISubscription SubscribeTo<T>(Action<T> aCallBack, Predicate<T> aPredicate) where T : INotification
        {
            if (aCallBack is null) throw new ArgumentNullException(nameof(aCallBack));

            Type iType = typeof(T);
            if (!this.InternalNotificationSubscriptions.Any(x => x == iType))
                this.InternalNotificationSubscriptions.Add(iType);

            NotificationCallback iNotifCallBack = NotificationCallback.Create(aCallBack, aPredicate);

            // Remove previous association if any
            //_ = this.NotificationCallbacks.RemoveAll(x => x.NotificationType == iNotifCallBack.NotificationType);

            // Add new one
            this.NotificationCallbacks.Add(iNotifCallBack);

            return this;
        }
        public ISubscription UnsubscribeFrom<T>() where T : INotification
        {
            Type iType = typeof(T);
            _ = this.InternalNotificationSubscriptions.RemoveAll(x => x == iType);
            _ = this.NotificationCallbacks.RemoveAll(x => x.NotificationType == iType);
            return this;
        }

        public ISubscription SubscribeTo<TIn, TOut>(Func<TIn, TOut> aCallback) where TIn : IRequest where TOut : IResponse
        {
            Type iTIn = typeof(TIn);
            Type iTOut = typeof(TOut);
            if (!this.InternalRequestResponsesSubscriptions.Any(x => x.Key == iTIn && x.Value == iTOut))
                this.InternalRequestResponsesSubscriptions.Add(new KeyValuePair<Type, Type>(iTIn, iTOut));

            RequestResponseCallback iCallback = RequestResponseCallback.Create(aCallback);

            // Remove previous association if any
            _ = this.RequestResponseCallbacks.RemoveAll(x => x.RequestType == iCallback.RequestType && x.ResponseType == iCallback.ResponseType);

            // Add new one
            this.RequestResponseCallbacks.Add(iCallback);
            return this;
        }        
        public ISubscription UnsubscribeFrom<TIn, TOut>() where TIn : IRequest where TOut : IResponse
        {
            Type iTIn = typeof(TIn);
            Type iTOut = typeof(TOut);
            _ = this.InternalRequestResponsesSubscriptions.RemoveAll(x => x.Key == iTIn && x.Value == iTOut);
            _ = this.RequestResponseCallbacks.RemoveAll(x => x.RequestType == iTIn && x.ResponseType == iTOut);
            return this;
        }


        private IEnumerable<Task> GetCallBackTaskEnum(INotification aNotification)
        {
            return this.NotificationCallbacks
                .Where(x => x.NotificationType == aNotification.GetType() 
                            && x.SafeInvokePredicate(aNotification))
                .Select(x => Task.Run(() => x.SafeInvokeCallBack(aNotification)));
        }

        


        #endregion ISubscription


        private List<Type> InternalNotificationSubscriptions { get; }
        private List<KeyValuePair<Type, Type>> InternalRequestResponsesSubscriptions { get; }
        public List<NotificationCallback> NotificationCallbacks { get; }
        private List<RequestResponseCallback> RequestResponseCallbacks { get; }
        private ConcurrentBag<Task> CallbacksTasks { get; }
        private Timer _CleanCallBackTasksTimer;


        public Subscription()
        {
            SubscriptionId = Guid.NewGuid();

            this.InternalNotificationSubscriptions = new List<Type>();
            this.InternalRequestResponsesSubscriptions = new List<KeyValuePair<Type, Type>>();
            this.NotificationCallbacks = new List<NotificationCallback>();
            this.RequestResponseCallbacks = new List<RequestResponseCallback>();
            this.CallbacksTasks = new ConcurrentBag<Task>();
            this._CleanCallBackTasksTimer = new Timer(30000)
            {
                Enabled = true
            };
            this._CleanCallBackTasksTimer.Elapsed += _CleanCallBackTasksTimer_Elapsed;

            //this.NotificationSubscriptions = InternalNotificationSubscriptions.AsReadOnly();
            //this.RequestResponsesSubscriptions = new ReadOnlyDictionary<IRequest, IResponse>(InternalRequestResponsesSubscriptions);
        }

        private void _CleanCallBackTasksTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.CallbacksTasks
                .Where(x => x.IsCompleted)
                .ToList()
                .ForEach(x => _ = this.CallbacksTasks.TryTake(out x));
        }

        
    }
}
