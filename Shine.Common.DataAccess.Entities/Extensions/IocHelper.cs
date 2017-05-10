using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Shine.Common.DataAccess.Entities.Extensions
{
    /// <summary>依赖注入操作辅助方法</summary>
    public static class IocHelper
    {
        /// <summary>根据接口定义创建实例</summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T CreateInstanceByDefinition<T>(params object[] args)
        {
            return (T)IocHelper.CreateInstanceByDefinition<T>(false, args);
        }

        /// <summary>根据接口定义创建实例</summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static object CreateRepositoryInstanceByLogicDefinition<T>(params object[] args)
        {
            return IocHelper.CreateInstanceByDefinition<T>(true, args);
        }

        /// <summary>根据接口类型获取实现类型</summary>
        /// <returns></returns>
        public static Type GetImplementType<T>(bool isCreateRepositoryByLogicDefinition)
        {
            Type type1 = typeof(T);
            if (type1.Namespace == null)
                return (Type)null;
            string str1 = type1.Namespace.Replace("Definition", "Implement");
            string name1 = type1.Name;
            string str2 = (int)name1[0] == 73 ? name1.Substring(1, name1.Length - 1) : name1;
            string str3 = (type1.Assembly.GetName().Name + ".dll").Replace("Definition", "Implement");
            if (isCreateRepositoryByLogicDefinition)
            {
                str1 = str1.Replace("Business", "DataAccess");
                str2 = str2.Replace("Logic", "Repository");
                str3 = str3.Replace("Business", "DataAccess");
            }
            string name2 = string.Format("{0}.{1}", (object)str1, (object)str2);
            string baseDirectory1 = AppDomain.CurrentDomain.BaseDirectory;
            if (!baseDirectory1.EndsWith("\\"))
                baseDirectory1 += "\\";
            if (!baseDirectory1.Contains("\\bin\\"))
                baseDirectory1 += "bin\\";
            string assemblyFile = baseDirectory1 + str3;
            Assembly assembly;
            try
            {
                assembly = Assembly.LoadFrom(assemblyFile);
            }
            catch (FileNotFoundException ex)
            {
                string baseDirectory2 = AppDomain.CurrentDomain.BaseDirectory;
                if (!baseDirectory2.EndsWith("\\"))
                    baseDirectory2 += "\\";
                assembly = Assembly.LoadFrom(baseDirectory2 + str3);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("接口【{0}】没有配置依赖注入，也未能根据名称找到它所在的程序集【{1}】", (object)typeof(T).FullName, (object)assemblyFile), ex);
            }
            Type type2 = assembly.GetType(name2);
            if (type2 == (Type)null)
                throw new Exception(string.Format("接口【{0}】没有配置依赖注入，也未能根据名称推导出它的实现。程序集名：{1},类名：{2}", (object)typeof(T).FullName, (object)str3, (object)name2));
            return type2;
        }

        /// <summary>根据接口定义创建实例</summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        private static object CreateInstanceByDefinition<T>(bool isCreateRepositoryByLogicDefinition, params object[] args)
        {
            return Activator.CreateInstance(IocHelper.GetImplementType<T>(isCreateRepositoryByLogicDefinition), args);
        }
    }
}
