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

namespace TCode
{
    /// <summary>
    /// demo.xaml 的交互逻辑
    /// </summary>
    public partial class demo : Window
    {
        public demo()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //HLImageButton btn = new HLImageButton();
            //btn.Content = "AAAAA";
            //btn.OnApplyTemplate();
            //this.AddChild(btn);
           
        }

        private void TImageButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("OK");
        }
    }
}
