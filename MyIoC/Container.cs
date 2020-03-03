using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace MyIoC
{
	public class Container
    {
        private AssemblyBuilder dynamicAssembly;
        private ModuleBuilder moduleBuilder;
		List<Type> assembliesList = new List<Type>();
		List<Type> typesList = new List<Type>();
		Dictionary<Type, Type> typesDictionary = new Dictionary<Type, Type>();
		public void AddAssembly(Assembly assembly)
        {
			AppDomain currentDomain = AppDomain.CurrentDomain;
            AssemblyName assemName = new AssemblyName();
            assemName.Name = assembly.GetName().ToString();
            dynamicAssembly = currentDomain.DefineDynamicAssembly(assemName, AssemblyBuilderAccess.Run);
            moduleBuilder = dynamicAssembly.DefineDynamicModule("MyModule");
        }

        public void AddType(Type type)
        {
            typesList.Add(moduleBuilder.DefineType(type.Name));
        }

        public void AddType(Type type, Type baseType)
        {
            typesDictionary.Add(baseType, moduleBuilder.DefineType(type.Name, TypeAttributes.Class, baseType));
        }

		public object CreateInstance(Type type)
		{
			return Activator.CreateInstance(typeof(Type));
		}

		public T CreateInstance<T>()
		{
			return Activator.CreateInstance<T>();
		}


		public void Sample()
		{
			var container = new Container();
            container.AddAssembly(Assembly.GetExecutingAssembly());

            var customerBLL = (CustomerBLL)container.CreateInstance(typeof(CustomerBLL));
			var customerBLL2 = container.CreateInstance<CustomerBLL>();

			container.AddType(typeof(CustomerBLL));
			container.AddType(typeof(Logger));
			container.AddType(typeof(CustomerDAL), typeof(ICustomerDAL));
		}
	}
}
