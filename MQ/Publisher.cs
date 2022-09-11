using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQ
{
    class Publisher
    {

        //private readonly IClientsManager _ClientsManager;

        //public Publisher(IClientsManager aClientsManager)
        //{
        //    _ClientsManager = aClientsManager ?? throw new ArgumentNullException(nameof(aClientsManager));
        //}


        //public async Task PublishAsync(IMessage aMessage)
        //{
        //    // Publish message to subscribed clients
        //    IEnumerable<ISubscription> iClientSubs = await _ClientsManager.GetByMessageTypeAsync(aMessage.Type);
        //    await Task.Run(() =>
        //    {
        //        iClientSubs.ToList().ForEach(x => x.OnMessageReceive(aMessage));
        //    });
        //}
    }
}
