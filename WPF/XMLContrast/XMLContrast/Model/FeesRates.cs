using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XMLContrast
{
    public class FeesRates : BaseEntity
    {
        /// <summary>
        /// 结构
        /// </summary>
        public string Structure { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public string TNumber { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string TProjectName { get; set; }
        /// <summary>
        /// 计算基础
        /// </summary>
        public string TCalculatedBasis { get; set; }
        /// <summary>
        /// 费率
        /// </summary>
        public string TRate { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public string BNumber { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string BProjectName { get; set; }
        /// <summary>
        /// 计算基础
        /// </summary>
        public string BCalculatedBasis { get; set; }
        /// <summary>
        /// 费率
        /// </summary>
        public string BRate { get; set; }
        /// <summary>
        /// 检查结果
        /// </summary>
        public string CheckResult { get; set; }
        /// <summary>
        /// 组名
        /// </summary>
        public string GroupName { get; set; }
        //序号 项目名称  计算基础 费率 检查结果
    }
}
