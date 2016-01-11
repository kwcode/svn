using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace TCode
{
    /// <summary>
    /// 用于对TreeView控件进行数据绑定的类，数据绑定后对TreeView的各种操作会变得相对容易些
    /// INotifyPropertyChanged接口用于通知checkbox状态改变
    /// </summary>
    public class TreeItem : INotifyPropertyChanged
    {
        private ObservableCollection<TreeItem> _children;

        public TreeItem()
        {
            _children = new ObservableCollection<TreeItem>();
        }
        public SqlConnectResult SqlConnectResult { get; set; }
        /// <summary>
        /// 节点类型
        /// </summary>
        public NodeType NodeType { get; set; }
        public int Width { get; set; }
        //
        //  节点文本
        public string NodeText { get; set; }
        //
        //  节点值
        public object NodeTag { get; set; }
        //
        //  节点图标路径
        public string ItemIcom { get; set; }
        //
        //  父节点
        public TreeItem Parent { get; set; }
        //

        public string ParentID { get; set; }
        //  子节点
        public ObservableCollection<TreeItem> Children
        {
            get { return _children; }
            set { _children = value; }
        }

        public TreeItem This { get { return this; } }

        //节点文本颜色
        public string Foreground { get; set; }

        /// <summary>
        /// 是否展开节点
        /// </summary>
        private bool _IsExpanded = false;
        public bool IsExpanded
        {
            get { return _IsExpanded; }
            set { _IsExpanded = value; }
        }
        //是否选中
        private bool _IsSelected = false;
        /// <summary>
        /// 是否选中行
        /// </summary>
        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                _IsSelected = value;
                this.OnPropertyChanged("IsSelected");
            }
        }
        private void OnPropertyChanged(string prop)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }

    public enum NodeType
    {
        服务器 = 0,
        数据库 = 1,
        系统表 = 2,
        用户表 = 3,
        表列 = 4,
    }
}
