using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Shine.Common.Business.Definition;
using Shine.Common.DataAccess.Definition;
using Shine.Common.DataAccess.Entities;
using Shine.Common.DataAccess.Entities.Extensions;

namespace Shine.Common.Business.Implement
{
    /// <summary>业务逻辑类工厂</summary>
    public class BusinessLogicFactory
    {
        private static readonly IList<IUnityContainer> containers;

        static BusinessLogicFactory()
        {
            IEnumerator enumerator = ConfigurationManager.OpenExeConfiguration("").Sections.GetEnumerator();
            IList<UnityConfigurationSection> source = (IList<UnityConfigurationSection>)new List<UnityConfigurationSection>();
            while (enumerator.MoveNext())
            {
                UnityConfigurationSection current = enumerator.Current as UnityConfigurationSection;
                if (current != null)
                    source.Add(current);
            }
            BusinessLogicFactory.containers = (IList<IUnityContainer>)source.Select<UnityConfigurationSection, IUnityContainer>((Func<UnityConfigurationSection, IUnityContainer>)(f => new UnityContainer().LoadConfiguration(f))).ToList<IUnityContainer>();
        }

        /// <summary>获取容器</summary>
        /// <typeparam name="TLogic">The type of the repository.</typeparam>
        /// <returns></returns>
        private static IUnityContainer GetContainer<TLogic>()
        {
            return BusinessLogicFactory.containers.FirstOrDefault<IUnityContainer>((Func<IUnityContainer, bool>)(f => f.Registrations.Any<ContainerRegistration>((Func<ContainerRegistration, bool>)(r => r.RegisteredType == typeof(TLogic)))));
        }

        /// <summary>获取业务逻辑实例</summary>
        /// <typeparam name="TLogic">业务逻辑接口</typeparam>
        /// <typeparam name="TEntity">业务实体</typeparam>
        /// <returns>务逻辑实例</returns>
        public static TLogic GetLogicInstance<TLogic, TEntity>() where TLogic : IEntityBaseLogic<TEntity> where TEntity : IEntityBase
        {
            return BusinessLogicFactory.GetInstance<TLogic>(false);
        }

        /// <summary>获取业务逻辑实例</summary>
        /// <typeparam name="TLogic">业务逻辑接口</typeparam>
        /// <returns>务逻辑实例</returns>
        public static TLogic GetLogicInstance<TLogic>()
        {
            return BusinessLogicFactory.GetInstance<TLogic>(false);
        }

        /// <summary>获取业务逻辑实例</summary>
        /// <typeparam name="TLogic">业务逻辑接口</typeparam>
        /// <param name="name">别名</param>
        /// <returns>数据访问实例</returns>
        public static TLogic GetLogicInstance<TLogic>(string name)
        {
            IUnityContainer container = BusinessLogicFactory.GetContainer<TLogic>();
            if (container != null)
                return container.Resolve<TLogic>(name, new ResolverOverride[0]);
            return IocHelper.CreateInstanceByDefinition<TLogic>();
        }

        /// <summary>获取数据访问实例</summary>
        /// <typeparam name="TRepository">数据访问接口</typeparam>
        /// <typeparam name="TEntity">业务实体</typeparam>
        /// <returns>数据访问实例</returns>
        public static TRepository GetRepositoryInstance<TRepository, TEntity>() where TRepository : IRepository<TEntity> where TEntity : IEntityBase
        {
            return BusinessLogicFactory.GetInstance<TRepository>(true);
        }

        /// <summary>获取数据访问实例</summary>
        /// <typeparam name="TRepository">数据访问接口</typeparam>
        /// <returns>数据访问实例</returns>
        public static TRepository GetRepositoryInstance<TRepository>()
        {
            return BusinessLogicFactory.GetInstance<TRepository>(true);
        }

        /// <summary>获取实例</summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>实例</returns>
        private static T GetInstance<T>(bool isGetRepository)
        {
            if (!typeof(T).IsInterface)
                throw new InvalidOperationException(string.Format("传入的类型{0}不是接口", (object)typeof(T).FullName));
            IUnityContainer container = BusinessLogicFactory.containers.FirstOrDefault<IUnityContainer>((Func<IUnityContainer, bool>)(f => f.Registrations.Any<ContainerRegistration>((Func<ContainerRegistration, bool>)(r => r.RegisteredType == typeof(T)))));
            if (container != null)
                return container.Resolve<T>();
            T obj;
            if (isGetRepository)
            {
                obj = IocHelper.CreateInstanceByDefinition<T>();
            }
            else
            {
                Type implementType = IocHelper.GetImplementType<T>(false);
                if (((IEnumerable<ConstructorInfo>)implementType.GetConstructors()).Any<ConstructorInfo>((Func<ConstructorInfo, bool>)(f => ((IEnumerable<ParameterInfo>)f.GetParameters()).Count<ParameterInfo>() == 1)))
                {
                    object byLogicDefinition = IocHelper.CreateRepositoryInstanceByLogicDefinition<T>();
                    obj = (T)Activator.CreateInstance(implementType, new object[1]
                    {
            byLogicDefinition
                    });
                }
                else
                    obj = (T)Activator.CreateInstance(implementType);
            }
            return obj;
        }
    }
}
