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

namespace WPF
{
    /// <summary>
    /// CustSlideDate.xaml 的交互逻辑
    /// </summary>
    public partial class CustSlideDate : UserControl
    {
        public CustSlideDate()
        {
            InitializeComponent();
            Init();
        }
        private void Init()
        {

            for (int i = -7; i < 7; i++)
            {
                string time = DateTime.Now.AddDays(i).ToString("yyyy-MM-dd");
                AddPanel(time);
            }
        }
        private void AddPanel(string content)
        {
            Button btn = new Button();
            btn.Margin = new Thickness(2);
            btn.Content = content;
            btn.Click += new RoutedEventHandler(btn_Click);
            stackPanelk.Children.Add(btn);
        }
        void btn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (DoClickEvent != null)
                DoClickEvent(btn.Content);
        }
        public event DoClick DoClickEvent;
    }
    public delegate void DoClick(object sender);
}
