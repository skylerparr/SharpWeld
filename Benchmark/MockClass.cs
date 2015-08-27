using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpWeld.Attributes;
using System.Reflection;

namespace Benchmarking
{
    public abstract class MockClass
    {
        public abstract string FirstName { get; set; }

        public abstract string LastName { set; }

        public abstract string MiddleName { get; }

        public abstract string FindEmployer(string street);

        public virtual string GetMiddleName()
        {
            return MiddleName;
        }

        [MockAbstractMethod]
        public abstract string GetNextEmployerName();

        public virtual string TestGetEmployer()
        {
            string name = GetNextEmployerName();
            return name;
        }

        [MockAbstractMethod2]
        public abstract string GetTitleAndPosition(string title, int position);

        public Object GetSomeCrazyObject(Object obj, long count)
        {
            return obj;
        }

        [Subtract]
        public abstract double Subtract(int arg1, int arg2, int arg3, int arg4);

        [Addition]
        public abstract double Add(int arg1, int arg2, int arg3, int arg4);

        public double GetZero()
        {
            return Add(5, 4, 3, 2) + Subtract(5, 4, 3, 2);
        }

        public class MockAbstractMethodAttribute : MethodAttribute
        {
            public override void DecorateMethod(object obj, MethodInfo methodInfo)
            {
                //sets the convention method to be called from the method all in the abstract method generation
                string name = methodInfo.Name;
                MethodInfo method = obj.GetType().GetMethod("__" + name);
                Func<string> implMethod = RunAttributeImplementedMethod;
                method.Invoke(obj, new Object[1] { implMethod });
            }

            public string RunAttributeImplementedMethod()
            {
                return "The Next Employer";
            }
        }

        public class MockAbstractMethod2Attribute : MethodAttribute
        {
            public override void DecorateMethod(object obj, MethodInfo methodInfo)
            {
                //sets the convention method to be called from the method all in the abstract method generation
                string name = methodInfo.Name;
                MethodInfo method = obj.GetType().GetMethod("__" + name);
                Func<string, int, string> implMethod = RunAttributeImplementedMethodWithArgs;
                method.Invoke(obj, new Object[1] { implMethod });
            }

            public string RunAttributeImplementedMethodWithArgs(string name, int postition)
            {
                return name + " " + postition;
            }
        }

        public class SubtractAttribute : MethodAttribute
        {
            public override void DecorateMethod(object obj, MethodInfo methodInfo)
            {
                //sets the convention method to be called from the method all in the abstract method generation
                string name = methodInfo.Name;
                MethodInfo method = obj.GetType().GetMethod("__" + name);
                Func<int, int, int, int, double> implMethod = SomeCoolMath;
                method.Invoke(obj, new Object[1] { implMethod });

            }

            public double SomeCoolMath(int arg1, int arg2, int arg3, int arg4)
            {
                return -arg1 - arg2 - arg3 - arg4;
            }

        }

        public class AdditionAttribute : MethodAttribute
        {
            public override void DecorateMethod(object obj, MethodInfo methodInfo)
            {
                //sets the convention method to be called from the method all in the abstract method generation
                string name = methodInfo.Name;
                MethodInfo method = obj.GetType().GetMethod("__" + name);
                Func<int, int, int, int, double> implMethod = AddMath;
                method.Invoke(obj, new Object[1] { implMethod });

            }

            public double AddMath(int arg1, int arg2, int arg3, int arg4)
            {
                return arg1 + arg2 + arg3 + arg4;
            }

        }
    }
}
