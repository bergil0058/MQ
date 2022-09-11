using MQ.Messages.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQ.Interfaces.Handlers
{
    interface IRequestResponseHandler<T, IResponse> : IMessageHandler<IRequest> where T : IRequest
    {
    }
}
