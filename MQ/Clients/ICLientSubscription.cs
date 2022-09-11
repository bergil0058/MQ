using MQ.Clients.CallBacks;
using MQ.Clients.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQ.Clients
{
    interface IClientSubscription
    {
        ISubscription Subscription { get; }
        List<NotificationCallback> NotificationCallbacks { get; }
    }
}
