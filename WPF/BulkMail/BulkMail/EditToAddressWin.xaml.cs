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
using System.IO;
using System.Collections.ObjectModel;

namespace BulkMail
{
    /// <summary>
    /// EditToAddressWin.xaml 的交互逻辑
    /// </summary>
    public partial class EditToAddressWin : Window
    {
        public EditToAddressWin()
        {
            InitializeComponent();
            btn_Ok.Click += new RoutedEventHandler(btn_Ok_Click);
            btn_Close.Click += new RoutedEventHandler(btn_Close_Click);
        }
        public List<ToAddress> ToAddressList { get; set; }
        public ObservableCollection<ToAddress> ToAddressToOBC { get; set; }
        void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        void btn_Ok_Click(object sender, RoutedEventArgs e)
        {
            ToAddress to = new ToAddress();
            to.Name = txt_Name.Text;
            to.Address = txt_ToAddress.Text;
            to.Remark = txt_Remark.Text;
            to.ID = Guid.NewGuid().ToString();
            string path = System.AppDomain.CurrentDomain.BaseDirectory + "Resources";
            DirectoryInfo dir = new DirectoryInfo(path);
            if (dir.Exists == false)//如果不存在则创建
            {
                Directory.CreateDirectory(path);
            }
            path = path + @"\ToAddress.xml";
            ToAddressList.Add(to);
            ToAddressToOBC.Add(to);
            BMCommon.SaveToAddressXML(ToAddressList, path);
            MessageBox.Show(this, "保存成功！");
        }


    }
}
