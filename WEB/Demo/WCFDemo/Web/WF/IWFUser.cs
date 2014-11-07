using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WF
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IWFUser”。
    [ServiceContract]
    public interface IWFUser
    {
        /// <summary>
        /// 保存用户
        /// </summary>
        [OperationContract]
        int Save();

        /// <summary>
        /// 获取所有的用户
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        string GetUserList();
    }
}
