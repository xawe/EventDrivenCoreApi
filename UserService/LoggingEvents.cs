using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService
{
    public static class LoggingEvents
    {
        internal const int Create = 1000;
        internal const int Read = 1001;
        internal const int Update = 1002;
        internal const int Delete = 1003;

        internal const int Error = 3000;
        internal const int RecordNotFound = 4000;
    }
}
