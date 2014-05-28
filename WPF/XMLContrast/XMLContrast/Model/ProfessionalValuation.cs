using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XMLContrast
{
    public class ProfessionalValuation : BaseEntity
    {
        /// <summary>
        /// 招标名称
        /// </summary>
        public string TName { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public string TMoney { get; set; }
        /// <summary>
        /// 招标名称
        /// </summary>
        public string BName { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public string BMoney { get; set; }
        /// <summary>
        /// 检查结果
        /// </summary>
        public string CheckResult { get; set; }

    }
}
