using System.Linq;
using System.Reflection;
using UnityEngine;
using Type = System.Type;
using AppDomain = System.AppDomain;
using Activator = System.Activator;
using Generic = System.Collections.Generic;

namespace WebSdk.Core.Runtime.Helpers
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
            Debug.Log(" In ReflectionHelper.GetTypesWithAttribute");
            
            Generic.Dictionary<Type,T> all = new Generic.Dictionary<Type, T>();
            //var ass = GetAssemblyByName("Assembly-CSharp");
            
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
    
            Debug.Log($"Found {assemblies.Length} assemblies");
            
            foreach (var assembly in assemblies)
            {
                if(assembly.FullName.Contains("mscorlib") || 
                   assembly.FullName.Contains("UnityEngine") || 
                   assembly.FullName.Contains("UnityEditor") || 
                   assembly.FullName.Contains("System.") || 
                   assembly.FullName.Contains("System,") || 
                   assembly.FullName.Contains("Unity.") || 
                   assembly.FullName.Contains("Mono.")) continue;
                
                //Debug.Log($"Checking assembly {assembly.FullName}");
                foreach(Type type in assembly.GetTypes())
                {
                    object attribute = type.GetCustomAttribute(typeof(T), true);
                    if (attribute != null) 
                    {
                        Debug.Log($"------------- Found Type {type.Name}");
                        all.Add(type,(T) attribute);
                        break;
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