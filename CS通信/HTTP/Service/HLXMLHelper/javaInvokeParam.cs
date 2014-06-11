using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace BaseApiCommon
{
    [Serializable]
    public class javaInvokeParam
    {
        private String Interface;
        private String MethodName;

        /**
         * @return the interface
         */
        public String getInterface()
        {
            return Interface;
        }

        /**
         * @param interface1
         *            the interface to set
         */
        public void setInterface(String interface1)
        {
            Interface = interface1;
        }

        /**
         * @return the methodName
         */
        public String getMethodName()
        {
            return MethodName;
        }

        /**
         * @param methodName
         *            the methodName to set
         */
        public void setMethodName(String methodName)
        {
            MethodName = methodName;
        }

        public javaInvokeParam()
        {
        }

        public javaInvokeParam(String Interface, String MethodName)
        {
            this.Interface = Interface;
            this.MethodName = MethodName;
        }
    }
}