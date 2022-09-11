using MQ.Interfaces;
using MQ.Interfaces.Handlers;
using MQ.Messages.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQ.MessageHandlers
{
    class RequestHandler<T> : IMessageHandler<IRequest>
    {
    }
}
