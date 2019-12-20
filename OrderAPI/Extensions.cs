using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderAPI
{
    public static class Extensions
    {
        public static IEnumerable<Type> GetInterfaces(
            this Type type,
            bool includeInherited)
        {
            if (includeInherited || (object)type.BaseType == null)
                return (IEnumerable<Type>)type.GetInterfaces();
            return ((IEnumerable<Type>)type.GetInterfaces()).Except<Type>((IEnumerable<Type>)type.BaseType.GetInterfaces());
        }
    }
}