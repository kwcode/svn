using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataBack.Model
{

        #region DataBackup_Model
        public class DataBackup_Model
        {
            private string _id;
            private string _txtClass;
            private string _txtSourcePath;
            private string _txtTargetPath;

            public string txtTargetPath
            {
                get { return _txtTargetPath; }
                set { _txtTargetPath = value; }
            }

            public string txtSourcePath
            {
                get { return _txtSourcePath; }
                set { _txtSourcePath = value; }
            }

            public string id
            {
                get { return _id; }
                set { _id = value; }
            }

            public string txtClass
            {
                get { return _txtClass; }
                set { _txtClass = value; }
            }
        }
        #endregion



}
