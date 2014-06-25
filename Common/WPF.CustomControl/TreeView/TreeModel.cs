using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Reflection;
using System.Windows;

namespace WPF.CustomControl
{
    /// <summary>
    /// 用于对TreeView控件进行数据绑定的类，数据绑定后对TreeView的各种操作会变得相对容易些
    /// INotifyPropertyChanged接口用于通知checkbox状态改变
    /// </summary>
    public class TreeModel : INotifyPropertyChanged
    {
        public TreeModel()
        {
            _children = new ObservableCollection<TreeModel>();
        }
        private ObservableCollection<TreeModel> _children;
        public ObservableCollection<TreeModel> Children
        {
            get { return _children; }
            set { _children = value; OnPropertyChanged("Children"); }
        }
        public string ID { get; set; }
        public string _Code;
        public string Code { get { return _Code; } set { _Code = value; OnPropertyChanged("Code"); } }

        public string _Name;
        public string Name { get { return _Name; } set { _Name = value; OnPropertyChanged("Name"); } }
        /// <summary>
        ///  显示值 节点文本 一般是 code和name 组合而成  
        /// </summary>
        public string NodeText { get { return _NodeText; } set { _NodeText = value; OnPropertyChanged("NodeText"); } }
        private string _NodeText;
        /// <summary>
        /// 节点图片
        /// </summary>
        public string ItemIcon { get { return _ItemIcon; } set { _ItemIcon = value; OnPropertyChanged("ItemIcon"); } }
        public string _ItemIcon;


        public TreeModel Parent { get; set; }

        public TreeModel This { get { return this; } }
        public object obj { get; set; }

        private bool? _isChecked = false;
        /// <summary>
        ///  复选框 Check相关--------是否被选中 默认未被选中
        /// </summary>
        public bool? IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (!_IsAssociateChildren && !_IsAssociateParent)
                    this.SetIsChecked(value);
                else
                    if (value == false)
                    {
                        this.SetIsChecked(value, IsAssociateChildren, false);
                    }
                    else this.SetIsChecked(value, IsAssociateChildren, IsAssociateParent);

            }
        }

        #region 设置复选属性
        /// <summary>
        /// 是否单选--默认多选
        /// </summary>
        public bool IsRadio { get { return _isRadio; } set { _isRadio = value; } }
        private bool _isRadio = false;

        /// <summary>
        /// 是否关联父级 和  默认true
        /// </summary>
        public bool IsAssociateParent { get { return _IsAssociateParent; } set { _IsAssociateParent = value; } }
        private bool _IsAssociateParent = true;
        /// <summary>
        /// 是否关联子节点 和  默认true
        /// </summary>
        public bool IsAssociateChildren { get { return _IsAssociateChildren; } set { _IsAssociateChildren = value; } }
        public bool _IsAssociateChildren = true;

        #endregion

        #region 设置复选框的三态

        /// <summary>
        /// 设置Checked状态
        /// </summary>
        /// <param name="value">状态值</param>
        private void SetIsChecked(bool? value)
        {
            _isChecked = value;
            this.OnPropertyChanged("IsChecked");
        }

        /// <summary>
        /// 设置Checked状态
        /// </summary>
        /// <param name="value">状态值</param>
        /// <param name="updateChildren">是否引响子节点</param>
        /// <param name="updateParent">是否引响父节点</param>
        private void SetIsChecked(bool? value, bool updateChildren, bool updateParent)
        {
            if (value == _isChecked)
            {
                return;
            }
            _isChecked = value;
            if (updateChildren && _isChecked.HasValue)
            {
                foreach (TreeModel child in _children)
                {
                    child.SetIsChecked(_isChecked, true, false);
                }
            }
            if (updateParent && Parent != null)
            {
                Parent.VerifyCheckState();
            }
            this.OnPropertyChanged("IsChecked");
        }
        bool ParentStatus = true;
        /// <summary>
        /// 改变父节点的checked状态
        /// </summary>
        private void VerifyCheckState()
        {
            bool? state = null;
            for (int i = 0; i < this.Children.Count; i++)
            {
                bool? current = this.Children[i].IsChecked;
                if (i == 0)
                {
                    state = current;
                }
                else if (state != current)
                {
                    if (ParentStatus)
                    {
                        state = true;
                    }
                    else
                    {
                        state = null;
                    }
                    break;
                }
            }
            this.SetIsChecked(state, false, true);
        }
        #endregion

        private void OnPropertyChanged(string prop)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
