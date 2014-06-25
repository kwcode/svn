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
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Threading;
using BaseApiCommon;
using WPF.CustomControl;

namespace BulkMail
{
    /// <summary>
    /// RecipientAddressWin.xaml 的交互逻辑
    /// </summary>
    public partial class RecipientAddressWin : HLWindowExt
    {
        /// <summary>
        /// 接收者缓存
        /// </summary>
        private List<ToAddress> _ToList = new List<ToAddress>();//
        /// <summary>
        /// 接收者的地址集合
        /// </summary>
        private ObservableCollection<ToAddress> _ToOBC = new ObservableCollection<ToAddress>();

        public RecipientAddressWin()
        {
            InitializeComponent();
            LoadDataToUI();
            btn_Add.Click += new RoutedEventHandler(btn_Add_Click);
            btn_Refresh.Click += new RoutedEventHandler(btn_Refresh_Click);
            btn_Ok.Click += new RoutedEventHandler(btn_Ok_Click);

        }

        void btn_Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadDataToUI();
        }

        void btn_Add_Click(object sender, RoutedEventArgs e)
        {
            EditToAddressWin win = new EditToAddressWin();
            win.ToAddressList = _ToList;
            win.ToAddressToOBC = _ToOBC;
            win.Owner = this;
            win.ShowDialog();
        }

        public void LoadDataToUI()
        {
            StartProgress();
            new Thread(LoadData).Start();
        }
        private void LoadData()
        {
            try
            {
                string toPath = System.AppDomain.CurrentDomain.BaseDirectory + @"Resources\ToAddress.xml";
                BMCommon.ExistsPath(toPath);
                _ToList = BMCommon.GetToaddress(toPath).ToList();
                Global.SysContext.Send(o => { InitUI(); }, null);
            }
            catch (Exception ex)
            {
                Global.SysContext.Send(o => { MessageBox.Show(this, ex.Message); }, null);
            }
        }
        private void InitUI()
        {
            _ToOBC = new ObservableCollection<ToAddress>(_ToList);
            lv_Data.ItemsSource = _ToOBC;
            StopProgress();
        }
        #region 属性
        public string[] ToAddress { get; set; }
        #endregion
        #region 事件
        void btn_Ok_Click(object sender, RoutedEventArgs e)
        {
            List<string> toAddress = new List<string>();
            foreach (ToAddress item in _ToOBC)
            {
                if (item.IsCheck == true)
                {
                    toAddress.Add(item.Address);
                }
            }
            if (toAddress != null && toAddress.Count != 0)
            {
                ToAddress = toAddress.ToArray();
            }
            DialogResult = true;
        }
        #endregion
    }
}
