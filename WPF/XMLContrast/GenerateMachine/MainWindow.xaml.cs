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
using System.Management;
using WPF.CustomControl;
using System.Threading;
using System.Windows.Threading;

namespace GenerateMachine
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : HLWindowExt
    {
        public static SynchronizationContext SysContext = DispatcherSynchronizationContext.Current;
        public MainWindow()
        {
            InitializeComponent();
            btn_Generate.Click += new RoutedEventHandler(btn_Generate_Click);
        }

        void btn_Generate_Click(object sender, RoutedEventArgs e)
        {
            StartProgress();
            new Thread(Generate2).Start(); 
        }
        private void Generate2()
        {
            try
            {
                List<string> keys = KeyGenerator.GetKeys();
                Random rd = new Random();
                int index = rd.Next(0, keys.Count - 1);
                string str = keys[index].ToString();
                SysContext.Send(o => { txt_key.Text = str; StopProgress(); }, null);

            }
            catch { }

        }
        private void Generate1()
        {
            StartProgress();
            string str = "";
            new Thread(
                o =>
                {
                    str = KeyGenerator.getRNum();
                    SysContext.Send(a =>
                    {
                        txt_key.Text = str;
                        StopProgress();
                    }, null);
                }).Start();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
