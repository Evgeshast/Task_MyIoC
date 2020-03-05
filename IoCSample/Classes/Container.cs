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

        public Container()
        {
        }

        private void Initialize()
        {
            typesList.AddRange(GetTypesWith<ImportAttribute>(false));
            typesList.AddRange(GetTypesWith<ImportConstructorAttribute>(false));
            foreach (var type in GetTypesWith<ExportAttribute>(false))
            {
                typesDictionary.Add(type.BaseType, type);
            }
        }

        IEnumerable<Type> GetTypesWith<Attribute>(bool inherit)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes(), (a, t) => new { a, t })
                .Where(@t1 => @t1.t.IsDefined(typeof(Attribute), inherit))
                .Select(@t1 => @t1.t);
        }

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
            typesList.Add(type);
        }

        public void AddType(Type type, Type contract)
        {
            typesDictionary.Add(contract, type);
        }

		public object CreateInstance(Type type)
        {
            ConstructorInfo [] constructors = typeof(Type).GetConstructors();
            var constructorParameters = new List<Type>();
            foreach (var param in constructors[0].GetParameters())
            {
                var parameterType = typesList.FirstOrDefault(x => x == param.ParameterType);
                if(parameterType == null)
                {
                    if (typesDictionary.ContainsKey(parameterType))
                    {
                        parameterType = typesDictionary[param.ParameterType];
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                constructorParameters.Add(parameterType);
            }
            return Activator.CreateInstance(type, constructorParameters);
		}

		public T CreateInstance<T>()
		{
			return Activator.CreateInstance<T>();
		}
	}
}
