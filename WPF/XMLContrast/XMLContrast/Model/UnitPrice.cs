using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XMLContrast
{
    public class UnitPrice : BaseEntity
    {
        /// <summary>
        /// 结构
        /// </summary>
        public string Structure { get; set; }
        public string TName { get; set; }
        /// <summary>
        /// 工程量
        /// </summary>
        public string TEngineer { get; set; }

        public string TPrice { get; set; }

        public string BName { get; set; }
        /// <summary>
        /// 工程量
        /// </summary>
        public string BEngineer { get; set; }

        public string BPrice { get; set; }
        /// <summary>
        /// 检查结果
        /// </summary>
        public string CheckResult { get; set; }
        /// <summary>
        /// 组名
        /// </summary>
        public string GroupName { get; set; }
    }
}
