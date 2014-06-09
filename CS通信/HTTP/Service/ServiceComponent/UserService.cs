using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UserServiceInterface;

namespace ServiceComponent
{
    public class UserService : IUserService
    {
        public void Save(UserInfo user)
        {
            throw new NotImplementedException();
        }

        public UserInfo GetUserByID(string id)
        {
            throw new NotImplementedException();
        }

        public UserInfo[] GetAllUser()
        {
            List<UserInfo> list = new List<UserInfo>();
            list.Add(new UserInfo() { ID = Guid.NewGuid().ToString(), Code = "TKW", Name = "唐开文" });
            return list.ToArray();
        }
    }
}
