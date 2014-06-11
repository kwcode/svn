using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace htmlHelper
{
    [Serializable]
    public class person
    {
        // ID
        private int id;
        // 代码
        private String code;
        // 名字
        private String name;

        public int getId()
        {
            return id;
        }

        public void setId(int id)
        {
            this.id = id;
        }

        /**
         * @return the code
         */
        public String getCode()
        {
            return code;
        }

        /**
         * @param code
         *            the code to set
         */
        public void setCode(String code)
        {
            this.code = code;
        }

        /**
         * @return the name
         */
        public String getName()
        {
            return name;
        }

        /**
         * @param name
         *            the name to set
         */
        public void setName(String name)
        {
            this.name = name;
        }

        public person()
        {
        }

        public person(int id, String code, String name)
        {
            this.id = id;
            this.code = code;
            this.name = name;
        }
    }
}
