using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPF.CustomControl
{

    public class DTimer
    {
        public DTimer();

        public object Para { get; set; }

        public event Action<object> DoSomething;

        public void Start(double PeriodInMilliSeconds);
        public void Stop();
    }

}
