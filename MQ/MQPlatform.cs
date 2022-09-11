using MQ.Clients;
using MQ.Clients.Interfaces;
using MQ.Messages.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQ
{
    public class MQPlatform : INotifications
    {

        private static MQPlatform _Instance;
        private static MQPlatform Instance
        {
            get
            {
                if(_Instance == null)
                {
                    SubscriptionsManager iSubsManager = new SubscriptionsManager();
                    _Instance = new MQPlatform(iSubsManager);
                }
                return _Instance;
            }
            set
            {
                _Instance = value;
            }
        }
        private static INotifications InstanceNotifications => Instance;


        private readonly ISubscriptionsManager _ClientsManager;


        private MQPlatform(ISubscriptionsManager aClientsManager)
        {
            _ClientsManager = aClientsManager;
        }


        /// <summary>
        /// Returns after all suscribers have processed the notification
        /// </summary>
        /// <param name="aNotification"></param>
        /// <returns></returns>
        async Task INotifications.PublishAsync(INotification aNotification)
        {
            List<ISubscription> iSubscriptors = Instance._ClientsManager.GetSubscriptors(aNotification);

            // Get all callbacks and invoke them
            IEnumerable<Task> iCallBackTasks = iSubscriptors.SelectMany(x => GetCallBackTaskEnum(x, aNotification));

            await Task.WhenAll(iCallBackTasks);
        }
        /// <summary>
        /// Returns after all suscribers have processed the notification
        /// </summary>
        /// <param name="aNotification"></param>
        /// <returns></returns>
        public static async Task PublishAsync(INotification aNotification)
        {
            await InstanceNotifications.PublishAsync(aNotification);
        }
        private static IEnumerable<Task> GetCallBackTaskEnum(ISubscription aSubscription, INotification aNotification)
        {
            return aSubscription.NotificationCallbacks
                .Where(x => x.NotificationType == aNotification.GetType()
                            && x.SafeInvokePredicate(aNotification))
                .Select(x => Task.Run(() => x.SafeInvokeCallBack(aNotification)));
        }


        ///// <summary>
        ///// Returns inmediatly without waiting suscribers to receive notification
        ///// </summary>
        ///// <param name="aNotification"></param>
        //public void Publish(INotification aNotification)
        //{
        //    List<Task> iCallBackTasks = this.GetCallBackTaskEnum(aNotification).ToList();

        //    iCallBackTasks.ForEach(x => this.CallbacksTasks.Add(x));
        //}


        public static ISubscription Subscribe()
        {
            return Instance._ClientsManager.AddSubscription();
        }

        public void Unsubscribe(ISubscription aSubscription)
        {
            _ClientsManager.RemoveSubscription(aSubscription);
        }
    }
}
