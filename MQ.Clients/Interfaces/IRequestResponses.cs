using MQ.Messages.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQ.Clients.Interfaces
{
    public interface IRequestResponses
    {
        Task<TResponse> RequestAsync<TResponse>(IRequest aRequest) where TResponse : IMessage;

        /// <summary>
        /// Delegates called from MQ engine each time a new message is received by this subscription
        /// </summary>
        /// <param name="aMessage"></param>
        /// <returns></returns>
        //Task<Func<IRequest, IResponse>> RequestReceiveAsync { get; }
    }
}
