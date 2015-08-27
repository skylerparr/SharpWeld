using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpWeld
{
    public interface Decorator
    {
        T InitializeType<T>(Type type, Object[] args) where T : class, new();
        void Decorate<T>(T instance);
        void PreInitialize(Type type);
        void PostInitialIze(Object instance);
    }
}
