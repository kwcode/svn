using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections;
using Microsoft.Win32;

namespace WPF.CustomControl
{
    /// <summary>
    /// 打开文件控件
    /// </summary>
    public partial class OpenFileDialogCtrl : UserControl
    {
        public OpenFileDialogCtrl()
        {
            InitializeComponent();
            btn_open.Click += new RoutedEventHandler(btn_open_Click);
        }
        /// <summary>
        /// 打开按钮事件
        /// </summary>
        public event RoutedEventHandler DoButtonOpen;
        void btn_open_Click(object sender, RoutedEventArgs e)
        {
            if (DoButtonOpen == null)
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Multiselect = true;
                fileDialog.Title = "请选择文件";
                fileDialog.Filter = "DLL文件|*.DLL*|所有文件|*.*";
                bool? b = fileDialog.ShowDialog();
                if (b.Value)
                {
                    FileName = fileDialog.FileName;
                    SafeFileName = fileDialog.SafeFileName;
                    AddComBox(SafeFileName);
                    TextContent = SafeFileName;
                }
            }
            if (DoButtonOpen != null)
                DoButtonOpen(sender, e);
        }
        /// <summary>
        /// Combox控件显示值
        /// </summary>
        public string TextContent { get { return cb_text.Text; } set { cb_text.Text = value; } }
        /// <summary>
        /// 获取或设置用于生成的内容System.Windows.Controls.ItemsControl和集合。
        /// </summary>
        public IEnumerable ItemsSource { get { return cb_text.ItemsSource; } set { cb_text.ItemsSource = value; } }
        /// <summary>
        /// 取得使用生成内容的System.Windows.Controls.ItemsControl的收集。
        /// </summary>
        public ItemCollection Items
        {
            get { return cb_text.Items; }
        }
        public string FileName { get; set; }
        public string SafeFileName { get; set; }
        private void AddComBox(string item)
        {
            bool b = cb_text.Items.Contains(item);
            if (!b)
                cb_text.Items.Add(item);

        }
    }
}