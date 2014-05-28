using System;
using System.Runtime.Serialization;
namespace BaseApiCommon
{
    /// <summary>
    /// Class InvokeParam
    /// </summary>
    [DataContract]
    [Serializable]
    public sealed class InvokeParam
    {
        /// <summary>
        /// The interface
        /// </summary>
        [DataMember]
        public string Interface = string.Empty;
        /// <summary>
        /// The method name
        /// </summary>
        [DataMember]
        public string MethodName = string.Empty;
        /// <summary>
        /// The parameters
        /// </summary>
        [DataMember]
        public object[] Parameters = null;
    }
}
