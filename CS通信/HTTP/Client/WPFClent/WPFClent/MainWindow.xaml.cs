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
using System.Net;
using System.Security;
using System.IO;
using WPFClent;

namespace WPFHttpClent
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            BaseApiCommon.ServiceProxy.URL = "http://localhost:2015/HLService.aspx?Type=0";
        }
        private void btn_Invoke_Click(object sender, RoutedEventArgs e)
        {
            List<UserServiceInterface.UserInfo> list = Services.UserService.GetAllUser().ToList();
        }
    }
}
