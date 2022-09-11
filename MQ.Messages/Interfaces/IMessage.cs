using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQ.Messages.Interfaces
{
    public interface IMessage
    {
        string Type { get; }

        Guid SourceId { get; }
        string SourceName { get; }
    }
}
