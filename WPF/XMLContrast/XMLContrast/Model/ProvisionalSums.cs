using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XMLContrast
{
    /// <summary>
    /// 材料
    /// </summary>
    public class ProvisionalSums : BaseEntity
    {
        /// <summary>
        /// 招标序号
        /// </summary>
        public string TNumber { get; set; }
        /// <summary>
        /// 招标名称
        /// </summary>
        public string TName { get; set; }
        
        /// <summary>
        /// 招标单位
        /// </summary>
        public string TUnit { get; set; }
        /// <summary>
        /// 暂列金额
        /// </summary>
        public string TProvisionalSums { get; set; }

        /// <summary>
        /// 投标序号
        /// </summary>
        public string BNumber { get; set; }
        /// <summary>
        /// 投标名称
        /// </summary>
        public string BName { get; set; } 
        /// <summary>
        /// 投标单位
        /// </summary>
        public string BUnit { get; set; }
        /// <summary>
        /// 暂列金额
        /// </summary>
        public string BProvisionalSums { get; set; }

        /// <summary>
        /// 检查结果
        /// </summary>
        public string CheckResult { get; set; }
    }
}
