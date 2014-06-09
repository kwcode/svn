using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserServiceInterface
{
    [Serializable]
    public class UserInfo
    {
        public string ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int Sex { get; set; }
    }
}
