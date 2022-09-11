using MQ.Messages.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQ.Clients.Interfaces
{
    public interface INotifications
    {
        Task PublishAsync(INotification aNotification);
        void Publish(INotification aNotification);
        //Action<INotification> NotificationReceive { get; }
    }
}
