using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
using System.Reflection;

namespace BaseApiCommon
{
    public class RuntimeBuildCommon
    {
        /// <summary> 
        /// 在运行时定义并创建类的新实例。
        /// 这里是：【可以执行但无法保存该动态程序集。】
        /// </summary>
        /// <param name="assemblyName"> 程序集的显示名称动态程序集的唯一标识 </param>
        /// <param name="moduleName">该动态模块的名称。长度必须小于 260 个字符。 </param>
        /// <param name="typName">  类型的完整路径。name 不能包含嵌入的 null 值</param>
        /// <param name="interfaceTyp"> 此类型实现的接口Type typeFromHandle = typeof(TInterface);</param>
        /// <returns></returns>
        public static TypeBuilder CreateTypeBuilder(string assemblyName, string moduleName, string typName, Type interfaceTyp)
        {
            AssemblyName name = new AssemblyName(assemblyName);
            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(name, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(moduleName);
            TypeBuilder typeBuilder = moduleBuilder.DefineType(typName, TypeAttributes.Public);
            typeBuilder.AddInterfaceImplementation(interfaceTyp);
            return typeBuilder;
        }

        /// <summary>
        /// 获取接口方法的返回值、参数等信息。
        /// </summary>
        /// <param name="interfaceType">接口 Type typeFromHandle = typeof(TInterface);</param>
        /// <returns></returns>
        public static List<MethodInfo> GetInterfaceMethodInfos(Type interfaceType)
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
        /// 动态创建接口方法
        /// </summary>
        /// <param name="typBulder">在运行时定义并创建类的新实例</param>
        /// <param name="minfo">方法</param>
        /// <param name="interfaceName">接口名【FullName】</param> 
        /// <typeparam name="T">含有Invoke的类</typeparam>
        /// <param name="Invoke">调用的对应方法</param>
        public static void CreateInterfaceMethod<T>(TypeBuilder typBulder, MethodInfo minfo, string interfaceName, string Invoke)
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
            MethodInfo method = typeof(T).GetMethod(Invoke, BindingFlags.Static | BindingFlags.Public);
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

        
    }
}
