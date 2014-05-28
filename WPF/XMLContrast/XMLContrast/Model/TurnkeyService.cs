using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XMLContrast
{
    public class TurnkeyService : BaseEntity
    {
        public string TName { get; set; }
        public string TRate { get; set; }
        public string TServices { get; set; }
        public string TMoney { get; set; }

        public string BName { get; set; }
        public string BRate { get; set; }
        public string BServices { get; set; }
        public string BMoney { get; set; }

        /// <summary>
        /// 检查结果
        /// </summary>
        public string CheckResult { get; set; }
    }
}
