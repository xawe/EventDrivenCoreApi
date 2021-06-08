using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Message
{
    public interface IListener
    {
        void StartListener(string sqlConncetionString);
    }
}
