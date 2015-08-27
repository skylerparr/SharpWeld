using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpWeld.Attributes;
using System.Reflection;

namespace SharpWeld.CustomAttributes
{
    public abstract class AbstractMethodHandlerAttribute : MethodAttribute
    {
        public override void DecorateMethod(object obj, System.Reflection.MethodInfo methodInfo)
        {
            //sets the convention method to be called from the method all in the abstract method generation
            string name = methodInfo.Name;
            MethodInfo method = obj.GetType().GetMethod("__" + name);
            Object implMethod = GetHandler();
            method.Invoke(obj, new Object[1] { implMethod });
        }

        protected abstract Object GetHandler();
    }
}
