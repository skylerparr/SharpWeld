using System;
using SharpWeld.Attributes;
using System.Reflection;

namespace SharpWeldTests.Mocks
{
	public class MockAttributes
	{
		public MockAttributes ()
		{
		}
	}

	public class MockAbstractMethodAttribute : MethodAttribute
	{
		public override void DecorateMethod(object obj, MethodInfo methodInfo)
		{
			//sets the convention method to be called from the method all in the abstract method generation
			string name = methodInfo.Name;
			MethodInfo method = obj.GetType().GetMethod("__" + name);
			Func<string> implMethod = RunAttributeImplementedMethod;
			method.Invoke(obj, new Object[1]{implMethod});
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

