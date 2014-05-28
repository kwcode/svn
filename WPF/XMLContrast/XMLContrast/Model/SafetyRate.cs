using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XMLContrast
{
    public class SafetyRate : BaseEntity
    {
        /// <summary>
        /// 结构
        /// </summary>
        public string TStructure { get; set; }

        /// <summary>
        /// 费率
        /// </summary>
        public string TRate { get; set; }
        /// <summary>
        /// 结构
        /// </summary>
        public string BStructure { get; set; }
        /// <summary>
        /// 费率
        /// </summary>
        public string BRate { get; set; }
        /// <summary>
        /// 检查结果
        /// </summary>
        public string CheckResult { get; set; }
    }
}
