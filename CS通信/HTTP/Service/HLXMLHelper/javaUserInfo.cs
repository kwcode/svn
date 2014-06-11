using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserServiceInterface
{
    [Serializable]
    public class javaUserInfo
    {
        private string _id;

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _code;

        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private int _age;

        public int Age
        {
            get { return _age; }
            set { _age = value; }
        }
        private int _sex;

        public int Sex
        {
            get { return _sex; }
            set { _sex = value; }
        }
    }
}
