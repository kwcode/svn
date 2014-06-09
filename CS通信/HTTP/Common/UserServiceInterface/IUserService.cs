using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SysCore;

namespace UserServiceInterface
{
    [ServiceContractAttribute]
    public interface IUserService
    {
        void Save(UserInfo user);
        UserInfo GetUserByID(string id);
        UserInfo[] GetAllUser();
    }
}
