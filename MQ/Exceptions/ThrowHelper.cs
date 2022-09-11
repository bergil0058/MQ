using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQ.Exceptions
{
    static class ThrowHelper
    {

        internal static void ThrowArgumentNullExceptionIfDefault<T>(T aObject)
        {
            ThrowArgumentNullExceptionIfDefault(aObject, nameof(aObject));
        }
        internal static void ThrowArgumentNullExceptionIfDefault<T>(T aObject, string aParamName)
        {
            ThrowArgumentNullExceptionIfDefault(aObject, aParamName, null);
        }
        internal static void ThrowArgumentNullExceptionIfDefault<T>(T aObject, string aParamName, string aMessage)
        {
            if (aObject.Equals(default(T))) throw new ArgumentNullException(aParamName, aMessage);
        }
    }
}
