using MQ.Messages.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQ.Clients.CallBacks
{
    public class NotificationCallback
    {
        public Type NotificationType { get; }
        private Action<INotification> CallBack { get; }
        private Predicate<INotification> Predicate { get; }


        private NotificationCallback(Type aNotificationType, Action<INotification> aCallBack) : this(aNotificationType, aCallBack, null) { }
        private NotificationCallback(Type aNotificationType, Action<INotification> aCallBack, Predicate<INotification> aPredicate)
        {
            //if (aCallback is null) throw new ArgumentNullException(nameof(aCallback));

            NotificationType = aNotificationType ?? throw new ArgumentNullException(nameof(aNotificationType));
            CallBack = aCallBack ?? throw new ArgumentNullException(nameof(aCallBack));
            Predicate = aPredicate;
        }


        internal static NotificationCallback Create<T>(Action<T> aCallBack) where T : INotification
        {
            return Create(aCallBack, null);
        }
        internal static NotificationCallback Create<T>(Action<T> aCallBack, Predicate<T> aPredicate) where T : INotification
        {
            Type iNotificationType = aCallBack.GetType().GenericTypeArguments.FirstOrDefault();
            Action<INotification> iNotificationCallBack = new Action<INotification>(x => aCallBack((T)x));
            Predicate<INotification> iNotificationPredicate = aPredicate is null ? null : new Predicate<INotification>(x => aPredicate((T)x));

            return new NotificationCallback(iNotificationType, iNotificationCallBack, iNotificationPredicate);
        }


        internal void SafeInvokeCallBack(INotification aNotification)
        {
            try
            {
                this.CallBack.Invoke(aNotification);
            }
            catch (Exception)
            {
                // TODO: Do something here
                //throw;
            }
        }
        internal bool SafeInvokePredicate(INotification aNotification)
        {
            bool iRetVal = false;
            try
            {
                iRetVal = this.Predicate is null ? true : this.Predicate.Invoke(aNotification);
            }
            catch (Exception ex)
            {
                // TODO: Do something here
                //throw;
            }
            return iRetVal;
        }

        //public class NotificationFoo : INotification
        //{
        //    public string Type => throw new NotImplementedException();

        //    public Guid SourceId => throw new NotImplementedException();

        //    public string SourceName => throw new NotImplementedException();

        //    public string hello = "hello";
        //}

    }
}
