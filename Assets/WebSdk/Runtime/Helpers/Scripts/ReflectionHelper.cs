using System.Linq;
using System.Reflection;
using Type = System.Type;
using AppDomain = System.AppDomain;
using Activator = System.Activator;
using Generic = System.Collections.Generic;

namespace Global.Helpers.Runtime
{
    public static class ReflectionHelper
    {
        public static Type FindType(string typeName)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            
            foreach (Assembly assembly in assemblies)
            {
                var allTypesInAssembly = assembly.GetTypes();
                foreach (Type type in allTypesInAssembly)
                {
                    string typeAsString = type.ToString();
                    string afterDot = typeAsString.After(".");
                    
                    if (afterDot.Equals(typeName))
                    {
                        //ExternalHelpers.logger.Send($"::ReflectionHelper.FindType:: Found Object of Type {typeName}");
                        return type;
                    }
                }
            }

            return null;
        }
        
        public static Generic.Dictionary<Type, T> GetTypesWithAttribute<T>() 
        {
            Generic.Dictionary<Type,T> all = new Generic.Dictionary<Type, T>();
            //var ass = GetAssemblyByName("Assembly-CSharp");
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            
            foreach (var assembly in assemblies)
            {
                foreach(Type type in assembly.GetTypes())
                {
                    object attribute = type.GetCustomAttribute(typeof(T), true);
                    if (attribute != null) 
                    {
                        all.Add(type,(T) attribute);
                    }
                }
            }
            
            return all;
        }
        
        public static Assembly GetAssemblyByName(string name)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SingleOrDefault(assembly => assembly.GetName().Name == name);
        }
        
        public static T CreateByType<T>(Type type)
        {
            if (type == null) 
            {
                //ExternalHelpers.logger.Send($"::ReflectionHelper.CreateByType:: Object Type == null");
                return default(T);
            }
         
            //ExternalHelpers.logger.Send($"::ReflectionHelper.CreateByType:: Serialized object of type {type}");
            T obj = (T) Activator.CreateInstance(type);
            
            return obj;
        }
    }
}