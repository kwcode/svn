using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XMLContrast
{
    public class EngineeringStructures : BaseEntity
    {
        public string ICO { get; set; }
        /// <summary>
        /// 招标工程名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 检查结果
        /// </summary>
        public string CheckResult { get; set; }

    }
}
