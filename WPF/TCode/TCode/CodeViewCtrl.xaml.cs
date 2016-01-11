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

namespace TCode
{
    /// <summary>
    /// CodeViewCtrl.xaml 的交互逻辑
    /// </summary>
    public partial class CodeViewCtrl : UserControl
    {
        public CodeViewCtrl()
        {
            InitializeComponent();
        }
        public string Text
        {
            set
            {
                txt_Code.Text = value;
            }
        }
        public string NSpace
        {
            get
            {
                return txt_nSpace.Text;
            }
        }
    }
}
