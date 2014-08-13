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
using System.IO;

namespace WPF
{
    /// <summary>
    /// CustRichTextBox.xaml 的交互逻辑
    /// </summary>
    public partial class CustRichTextBox : UserControl
    {

        public CustRichTextBox()
        {
            InitializeComponent();
            //  this.KeyDown += new KeyEventHandler(CustRichTextBox_KeyDown);
            MenuItem item = GenerateCustMenuItem(EditingCommands.ToggleBold, "/WPF.CustomControl;component/Resources/RichBoxIco/icon_B.png", "复制");
            CustMenu.Items.Add(item);   
        }

        void CustRichTextBox_KeyDown(object sender, KeyEventArgs e)
        {

            bool isKey = Keyboard.IsKeyDown(Key.LeftCtrl);
            if (isKey)
            {
                if (e.Key == Key.S)
                {
                    MessageBox.Show(RBContent.ToString());
                }
            }
        }
        public MenuItem GenerateCustMenuItem(RoutedUICommand command, string ico, object toolTip)
        {
            MenuItem item = new MenuItem();
            item.Command = command;
            Image img = new Image();
            img.Source = new BitmapImage(new Uri(ico, UriKind.RelativeOrAbsolute));
            item.Icon = img;
            item.Width = 25;
            item.ToolTip = toolTip;
            return item;
        }

        private string _RBContent;
        public string RBContent
        {
            get
            { 
                string rtf = string.Empty;
                TextRange textRange = new TextRange(rich_Box.Document.ContentStart, rich_Box.Document.ContentEnd);
                using (MemoryStream ms = new MemoryStream())
                {
                    textRange.Save(ms, System.Windows.DataFormats.Rtf);
                    ms.Seek(0, SeekOrigin.Begin);
                    StreamReader sr = new StreamReader(ms);
                    rtf = sr.ReadToEnd();
                }
                _RBContent = rtf;
                return _RBContent;
            }
            set
            {
                _RBContent = value;
                rich_Box.Document.Blocks.Clear();
                if (!string.IsNullOrEmpty(value))
                {
                    TextRange textRange = new TextRange(rich_Box.Document.ContentStart, rich_Box.Document.ContentEnd);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (StreamWriter sw = new StreamWriter(ms))
                        {
                            sw.Write(value);
                            sw.Flush();
                            ms.Seek(0, SeekOrigin.Begin);
                            textRange.Load(ms, DataFormats.Rtf);
                        }
                    }

                }
            }
        }
    }

}
