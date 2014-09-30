using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomaticSound
{
    public class Task
    {
        public int ID { get; set; }
        public IntPtr hWnd { get; set; }
        public string Title { get; set; }
    }
}
