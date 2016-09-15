using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMe.Core.Enums
{
    // There's no eventType that equals to two because there is no such event type on SharePoint side
    public enum EventType
    {
        Regular = 0,
        Recurrent = 1,
        Deleted = 3,
        Modified = 4
    }
}