using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.CodeDom.Compiler;

namespace BaseApiCommon
{
    public class AssemblyCommon
    {
        //添加对应目录下面的依赖程序集地址
        private static string _DllPath { get; set; }
        /// <summary>
        /// 反射需找是否继承接口
        /// </summary>
        /// <param name="path">DLL的完整地址</param>
        /// <param name="iface">实现的接口名</param>
        /// <param name="dllName">DLL名称</param>
        /// <returns></returns>
        public static bool FindAssembly(string path, string iface, string dllName)
        { 
            try
            {
                bool b = false;
                Assembly ass = Assembly.LoadFile(path);
                AssemblyName[] assName = ass.GetReferencedAssemblies();
                foreach (AssemblyName item in assName)
                {
                    byte[] keyToken = item.GetPublicKeyToken();
                    if (keyToken.Length == 0)
                    {
                        _DllPath = GetPath(path, dllName, item.Name);
                        AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve);
                        AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
                    }
                }
                Type[] types = ass.GetTypes();
                foreach (Type item in types)
                {
                    if (item.IsClass && !item.IsAbstract)
                    {
                        Type type = item.GetInterface(iface);
                        if (type != null)
                        {
                            b = true;
                            break;
                        }
                    }
                }
                return b;
            }
            catch (Exception ex)
            {
                throw new Exception("未能加载依赖文件或程序集 " + _DllPath + "，系统找不到指定的文件", ex);
            }
        }
        private static string GetPath(string oldpath, string oldDllName, string newDllName)
        {
            string path = null;
            int index = oldpath.IndexOf(oldDllName);
            path = oldpath.Substring(0, 15) + newDllName + ".DLL";
            return path;

        }
        static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            //手动指定程序集的位置 
            if (_DllPath != null && _DllPath != string.Empty)
                return Assembly.LoadFrom(_DllPath);
            return null;
        }


        public static object InvokeMember(string dllPath, string intfname, string methodname, params object[] param)
        {
            object objs = null;
            byte[] buffer = System.IO.File.ReadAllBytes(dllPath);
            Assembly assm = Assembly.Load(buffer);
            Type[] types = assm.GetTypes();
            foreach (Type t in types)
            {
                string fullname = t.FullName;
                if (t.IsClass && !t.IsAbstract)
                {
                    Type type = t.GetInterface(intfname);
                    if (type != null)
                    {
                        MethodInfo mi = type.GetMethod(methodname);
                        object obj = Activator.CreateInstance(t);
                         objs = mi.Invoke(obj, param);
                    }
                }
            }
            return objs;
        }
    }


}

