using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using Microsoft.Win32;
using System.Threading;

namespace XMLContrast
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            bool b = GenerateMachine.KeyGenerator.IsRegist();
            if (b)
            {
                ListAssistantWin win = new ListAssistantWin();
                win.Show();
            }
            else
            {
                CheckRegistWin win = new CheckRegistWin();
                win.Show();
            }
        }
    }

}
