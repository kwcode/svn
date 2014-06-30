using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.entity
{
    public class UserInfo
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public string Name { get; set; }
        public string NickName { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string SmallPhoto { get; set; }
    }
}