using System;
using System.Reflection;
using System.Collections.Generic;
namespace task05
{
    public class ClassAnalyzer
    {
        private Type _type;

        public ClassAnalyzer(Type type)
        {
            _type = type;
        }

        public IEnumerable<string> GetPublicMethods()
            => _type
            .GetMethods()
            .Where(m => m.IsPublic)
            .Select(m => m.Name);

        public IEnumerable<string> GetMethodParams(string methodname)
            => _type
            .GetMethod(methodname)
            ?.GetParameters()
            .Select(p => p.Name!) 
            ?? Enumerable.Empty<string>();

        public IEnumerable<string> GetAllFields()
            => _type
            .GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
            .Select(f => f.Name);

        public IEnumerable<string> GetProperties()
            => _type
            .GetProperties()
            .Select(p => p.Name);

        public bool HasAttribute<T>() where T : Attribute
            => Attribute.IsDefined(_type, typeof(T));
    }
}
