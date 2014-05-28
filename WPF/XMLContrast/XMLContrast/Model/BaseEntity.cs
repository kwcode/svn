using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Media;
using System.Data;
using System.Reflection;

namespace XMLContrast
{
    /// <summary>
    /// 用于显示列表集合
    /// </summary>
    public class BaseEntity : INotifyPropertyChanged
    {
        private void OnPropertyChanged(string prop)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        //public string _Name;
        //public string Name { get { return _Name; } set { _Name = value; OnPropertyChanged("Name"); } }
        /// <summary>
        /// 唯一标识号 默认1
        /// </summary>
        public int PrimaryKey { get { return _PrimaryKey; } set { _PrimaryKey = value; } }
        private int _PrimaryKey = 1;
        /// <summary>
        /// 节点图片
        /// </summary>
        public string ItemIcon { get { return _ItemIcon; } set { _ItemIcon = value; OnPropertyChanged("ItemIcon"); } }
        public string _ItemIcon;
        /// <summary>
        /// 文本颜色
        /// </summary>
        public Brush Foreground { get { return _Foreground; } set { _Foreground = value; OnPropertyChanged("Foreground"); } }
        public Brush _Foreground = Brushes.Black;
    }

}