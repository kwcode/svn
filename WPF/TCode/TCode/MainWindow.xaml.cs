using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TCode
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            initUI();
            CodeViewCtrl ctrl = new CodeViewCtrl();
            AddTabControl("代码", ctrl);

        }

        private void initUI()
        {
            this.WindowState = WindowState.Maximized;
           
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void AddTabControl(string header, object itemContent)
        {

            TabItem tItem = new TabItem();
            tItem.Header = header;
            tItem.Content = itemContent;
            tabControl.Items.Add(tItem);
        }
    }
}
