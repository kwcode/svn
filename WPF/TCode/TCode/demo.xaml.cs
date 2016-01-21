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
using TCode.Model;

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
            //BaseApiCommon.XmlCommon.Init();
            //BaseApiCommon.XmlCommon.ReadDB();
            try
            {
                Helper.SerializationCommon.Init();
                List<DB> dbList = Helper.SerializationCommon.DeserializeXML<List<DB>>();
                //if (dbList == null)
                //{
                //    dbList = new List<DB>();
                //}
                //dbList.Add(new DB() { ServiceName = "sa", UserID = "sa", Password = "123" });
                //Helper.SerializationCommon.SerializeXML(dbList);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void TImageButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("OK");
        }
    }
}
