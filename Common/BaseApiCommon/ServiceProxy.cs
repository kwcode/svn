using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
using System.Reflection;
using SysCore;
using System.Net;

namespace BaseApiCommon
{
    public sealed class ServiceProxy
    {
        /// <summary>
        /// 缓存实例 采用单利模式
        /// </summary>
        private static Dictionary<string, object> _Instances = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        private static object _SycInstances = new object();
        private static string _URL;
        /// <summary>
        /// 服务器地址
        /// </summary>
        public static string URL
        {
            get
            {
                return ServiceProxy._URL;
            }
            set
            {
                ServiceProxy._URL = value;
            }
        }

        #region 构造接口

        /// <summary>
        /// 构造传入的服务接口
        /// </summary>
        /// <typeparam name="TInterface">需要创建的接口类型</typeparam>
        /// <returns></returns>
        public static TInterface CreateInterface<TInterface>()
        {
            return ServiceProxy.CreateInterfaceImplementation<TInterface>();
        }
        /// <summary>
        /// 创建接口实例。
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <returns></returns>
        private static TInterface CreateInterfaceImplementation<TInterface>()
        {
            Type typeFromHandle = typeof(TInterface);
            if (!ServiceProxy._Instances.ContainsKey(typeFromHandle.FullName))
            {
                lock (ServiceProxy._SycInstances)
                {
                    if (!ServiceProxy._Instances.ContainsKey(typeFromHandle.FullName))
                    {
                        if (!ServiceProxy.IsServiceContract(typeFromHandle))
                        {
                            throw new Exception(typeFromHandle.FullName + "不是服务接口类型");
                        }
                        string text = typeFromHandle.ToString();
                        int num = text.LastIndexOf('.');
                        string fullName = typeFromHandle.FullName;
                        string text2 = text.Substring(num + 2);
                        string assemblyName = text.Substring(0, num) + "." + text2;
                        string moduleName = text.Substring(0, num) + "." + text2;
                        TypeBuilder typeBuilder = ServiceProxy.CreateTypeBuilder(assemblyName, moduleName, text2, typeFromHandle);
                        List<MethodInfo> interfaceMethodInfos = ServiceProxy.GetInterfaceMethodInfos(typeFromHandle);
                        ServiceProxy.CreateInterfaceMethods(interfaceMethodInfos, typeBuilder, fullName);
                        Type type = typeBuilder.CreateType();
                        ServiceProxy._Instances.Add(typeFromHandle.FullName, Activator.CreateInstance(type));
                    }
                }
            }
            return (TInterface)ServiceProxy._Instances[typeFromHandle.FullName];
        }

        private static void CreateInterfaceMethods(List<MethodInfo> methods, TypeBuilder typBuilder, string interfaceName)
        {
            foreach (MethodInfo current in methods)
            {
                ServiceProxy.CreateInterfaceMethod(typBuilder, current, interfaceName);
            }
        }

        /// <summary>
        /// 动态创建接口方法。
        /// </summary>
        /// <param name="typBulder"></param>
        /// <param name="minfo"></param>
        /// <param name="interfaceName"></param>
        private static void CreateInterfaceMethod(TypeBuilder typBulder, MethodInfo minfo, string interfaceName)
        {
            ParameterInfo[] parameters = minfo.GetParameters();
            int num = parameters.Length;
            EMethodType eMethodType = EMethodType.NoParaAndReturn;
            MethodBuilder methodBuilder = typBulder.DefineMethod(minfo.Name, MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Final | MethodAttributes.Virtual, minfo.ReturnType, (
                from r in parameters
                select r.ParameterType).ToArray<Type>());
            ILGenerator iLGenerator = methodBuilder.GetILGenerator();
            Label label = iLGenerator.DefineLabel();
            if (minfo.ReturnType != typeof(void))
            {
                iLGenerator.DeclareLocal(typeof(object));
                iLGenerator.DeclareLocal(minfo.ReturnType);
                eMethodType = ((num > 0) ? EMethodType.ParasAndReturn : EMethodType.Return);
            }
            else
            {
                eMethodType = ((num > 0) ? EMethodType.Paras : EMethodType.NoParaAndReturn);
            }
            if (num > 0)
            {
                iLGenerator.DeclareLocal(typeof(object[]));
            }
            MethodInfo method = typeof(ServiceProxy).GetMethod("Invoke", BindingFlags.Static | BindingFlags.Public);
            iLGenerator.Emit(OpCodes.Nop);
            iLGenerator.Emit(OpCodes.Ldstr, interfaceName);
            iLGenerator.Emit(OpCodes.Ldstr, minfo.Name);
            iLGenerator.Emit(OpCodes.Ldc_I4_S, num);
            iLGenerator.Emit(OpCodes.Newarr, typeof(object));
            if (eMethodType == EMethodType.ParasAndReturn)
            {
                iLGenerator.Emit(OpCodes.Stloc_2);
                for (int i = 0; i < num; i++)
                {
                    iLGenerator.Emit(OpCodes.Ldloc_2);
                    iLGenerator.Emit(OpCodes.Ldc_I4, i);
                    iLGenerator.Emit(OpCodes.Ldarg, i + 1);
                    if (parameters[i].ParameterType.IsValueType)
                    {
                        iLGenerator.Emit(OpCodes.Box, parameters[i].ParameterType);
                    }
                    iLGenerator.Emit(OpCodes.Stelem_Ref);
                }
                iLGenerator.Emit(OpCodes.Ldloc_2);
            }
            if (eMethodType == EMethodType.Paras)
            {
                iLGenerator.Emit(OpCodes.Stloc_0);
                for (int i = 0; i < num; i++)
                {
                    iLGenerator.Emit(OpCodes.Ldloc_0);
                    iLGenerator.Emit(OpCodes.Ldc_I4, i);
                    iLGenerator.Emit(OpCodes.Ldarg, i + 1);
                    if (parameters[i].ParameterType.IsValueType)
                    {
                        iLGenerator.Emit(OpCodes.Box, parameters[i].ParameterType);
                    }
                    iLGenerator.Emit(OpCodes.Stelem_Ref);
                }
                iLGenerator.Emit(OpCodes.Ldloc_0);
            }
            iLGenerator.Emit(OpCodes.Call, method);
            if (eMethodType == EMethodType.ParasAndReturn || eMethodType == EMethodType.Return)
            {
                iLGenerator.Emit(OpCodes.Stloc_0);
                iLGenerator.Emit(OpCodes.Ldloc_0);
                if (minfo.ReturnType.IsValueType)
                {
                    iLGenerator.Emit(OpCodes.Unbox_Any, minfo.ReturnType);
                }
                else
                {
                    iLGenerator.Emit(OpCodes.Isinst, minfo.ReturnType);
                }
                iLGenerator.Emit(OpCodes.Stloc_1);
                iLGenerator.Emit(OpCodes.Br_S, label);
                iLGenerator.MarkLabel(label);
                iLGenerator.Emit(OpCodes.Ldloc_1);
            }
            if (eMethodType == EMethodType.NoParaAndReturn || eMethodType == EMethodType.Paras)
            {
                iLGenerator.Emit(OpCodes.Pop);
            }
            iLGenerator.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// 获取接口方法的返回值、参数等信息。
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <returns></returns>
        private static List<MethodInfo> GetInterfaceMethodInfos(Type interfaceType)
        {
            List<MethodInfo> result = new List<MethodInfo>();
            MethodInfo[] methods = interfaceType.GetMethods();
            if (methods != null && methods.Length > 0)
            {
                result = methods.ToList<MethodInfo>();
            }
            return result;
        }
        /// <summary>
        /// 创建类型Builder。
        /// 在运行时定义并创建类的新实例。
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="moduleName"></param>
        /// <param name="typName"></param>
        /// <param name="interfaceTyp"></param>
        /// <returns></returns>
        private static TypeBuilder CreateTypeBuilder(string assemblyName, string moduleName, string typName, Type interfaceTyp)
        {
            AssemblyName name = new AssemblyName(assemblyName);
            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(name, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(moduleName);
            TypeBuilder typeBuilder = moduleBuilder.DefineType(typName, TypeAttributes.Public);
            typeBuilder.AddInterfaceImplementation(interfaceTyp);
            return typeBuilder;
        }
        private static bool IsServiceContract(Type intfType)
        {
            bool result;
            if (!intfType.IsInterface)
            {
                result = false;
            }
            else
            {
                object[] customAttributes = intfType.GetCustomAttributes(typeof(ServiceContractAttribute), false);
                object[] array = customAttributes;
                for (int i = 0; i < array.Length; i++)
                {
                    object obj = array[i];
                    if (obj is ServiceContractAttribute)
                    {
                        result = true;
                        return result;
                    }
                }
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 调用服务端指定接口的方法. 内部使用, 使用者不要直接使用
        /// </summary>
        /// <param name="intfname"></param>
        /// <param name="methodname"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static object Invoke(string intfname, string methodname, params object[] param)
        {
            object result = "";
            byte[] buffer = BaseApiCommon.SerializationCommon.Serilize(new InvokeParam
            {
                Interface = intfname,
                MethodName = methodname,
                Parameters = param
            });
            using (WebClient webClient = new WebClient())
            {
                byte[] buff = BaseApiCommon.WebClientCommon.GetFromServer(_URL, buffer);
                result = BaseApiCommon.SerializationCommon.Deserilize(buff);
                //  return result;
            }
            return result;
        }
        public byte[] InvokeServer(byte[] buffer)
        {
            //return this.RequestServer(1000, buffer);
            return new byte[0];
        }

        #endregion
    }
}
