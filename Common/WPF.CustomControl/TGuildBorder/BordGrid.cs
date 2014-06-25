using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPF.CustomControl
{
    /// <summary>
    /// 单元格信息
    /// </summary>
    public class BordGrid
    {
        public int rowIndex { get; set; }
        public int colIndex { get; set; }
        public TGridControl BorderControl { get; set; }
    }
}
