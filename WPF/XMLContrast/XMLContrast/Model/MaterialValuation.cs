using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XMLContrast
{
    public class MaterialValuation : BaseEntity
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
        /// 招标规格
        /// </summary>
        public string TSpecification { get; set; }
        /// <summary>
        /// 招标单位
        /// </summary>
        public string TUnit { get; set; }
        /// <summary>
        /// 招标型号
        /// </summary>
        public string TModel { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public string TPrice { get; set; }

        /// <summary>
        /// 投标序号
        /// </summary>
        public string BNumber { get; set; }
        /// <summary>
        /// 投标名称
        /// </summary>
        public string BName { get; set; }
        /// <summary>
        /// 投标规格
        /// </summary>
        public string BSpecification { get; set; }
        /// <summary>
        /// 投标单位
        /// </summary>
        public string BUnit { get; set; }
        /// <summary>
        /// 投标型号
        /// </summary>
        public string BModel { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public string BPrice { get; set; }
        /// <summary>
        /// 检查结果
        /// </summary>
        public string CheckResult { get; set; }
    }
}
