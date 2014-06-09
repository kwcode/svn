using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UserServiceInterface;
namespace WPFClent
{
    public class Services
    {
        private static object _LockObj = new object();
        private static IUserService _UserService = null;

        /// <summary>
        /// 访问接口
        /// </summary>
        public static IUserService UserService
        {
            get
            {
                if (_UserService == null)
                {
                    lock (_LockObj)
                    {
                        if (_UserService == null) _UserService = BaseApiCommon.ServiceProxy.CreateInterface<IUserService>();
                    }
                }
                return _UserService;
            }
        }
    }
}
