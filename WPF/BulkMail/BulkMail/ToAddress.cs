using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseApiCommon.Mail;
using System.ComponentModel;

namespace BulkMail
{
    [Serializable]
    public class ToAddress : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        //属性的变化
        public void OnPropertyChanged(string prop)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
        public string ID { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { get; set; }
        public string Address { get; set; }
        private bool _IsCheck;
        public bool IsCheck { get { return _IsCheck; } set { _IsCheck = value; OnPropertyChanged("IsCheck"); } }
        public string Remark { get; set; }
        public string Type { get; set; }
    }
}
