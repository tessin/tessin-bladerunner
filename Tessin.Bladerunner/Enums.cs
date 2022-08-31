using System;
using System.Linq;
using System.Reflection;

namespace Tessin.Bladerunner
{
    public static class Enums
    {
        public static (string, T)[] GetValues<T>(Type type)
        {	
            FieldInfo[] fields = type.GetFields();
            return fields.Where(e => e.Name != "value__").Select(e => (e.Name, (T) e.GetRawConstantValue())).ToArray();
        }
    }
}
