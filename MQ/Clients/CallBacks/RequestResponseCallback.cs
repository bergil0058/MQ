using MQ.Messages.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQ.Clients.CallBacks
{
    
    class RequestResponseCallback
    {
        public Type RequestType { get; }
        public Type ResponseType { get; }
        private Func<IRequest, IResponse> CallBack { get; }

        private RequestResponseCallback(Type aRequestType, Type aResponseType, Func<IRequest, IResponse> aCallback)
        {
            //if (aCallback is null) throw new ArgumentNullException(nameof(aCallback));

            RequestType = aRequestType ?? throw new ArgumentNullException(nameof(aRequestType));
            ResponseType = aResponseType ?? throw new ArgumentNullException(nameof(aResponseType));
            CallBack = aCallback ?? throw new ArgumentNullException(nameof(aCallback));
        }


        public static RequestResponseCallback Create<TIn, TOut>(Func<TIn, TOut> aCallBack) where TIn : IRequest where TOut : IResponse
        {
            Type[] aCallBackGenericTypeArguments = aCallBack.GetType().GenericTypeArguments;

            Type iRequestType = aCallBackGenericTypeArguments.FirstOrDefault();
            Type iResponseType = aCallBackGenericTypeArguments.LastOrDefault();

            Func<IRequest, IResponse> iCallBack = new Func<IRequest, IResponse>((x) => aCallBack((TIn)x));

            return new RequestResponseCallback(iRequestType, iResponseType, iCallBack);
        }


        public IResponse SafeInvokeCallBack(IRequest aRequest)
        {
            IResponse iRetVal = default(IResponse);
            try
            {
                iRetVal = this.CallBack.Invoke(aRequest);
            }
            catch (Exception)
            {

                throw;
            }
            return iRetVal;
        }
    }
}
