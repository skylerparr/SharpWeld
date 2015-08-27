using SharpWeld.ClassProvider;
using NUnit.Framework;
using System;
using System.Reflection;
using SharpWeldTest.Mocks;

namespace SharpWeldTest
{
    
    
    /// <summary>
    ///This is a test class for AssemblyProviderTest and is intended
    ///to contain all AssemblyProviderTest Unit Tests
    ///</summary>
	[TestFixture()]
    public class AssemblyProviderTest
    {

        private AssemblyBuilder assemblyProvider;

        //Use TestInitialize to run code before running each test
		[SetUp()]
        public void MyTestInitialize()
        {
            assemblyProvider = new AssemblyBuilder(new CSharpProvider());
        }
        //
        //Use TestCleanup to run code after each test has run
		[TearDown()]
        public void MyTestCleanup()
        {
            assemblyProvider = null;
        }
        //

        private string assemblyTest1 = "namespace SharpWeld.ClassProvider" +
            "{" +
            "    public class SampleClass" +
            "    {" +
            "        public string GetCrazyString() { return \"One Crazy String\"; }" +
            "    }" +
            "}";

        /// <summary>
        ///A test for BuildAssembly
        ///</summary>
		[Test()]
        public void ShouldReturnAssemblyThatCanBeInstantiated()
        {
            string code = assemblyTest1;
            Assembly actual;
            actual = assemblyProvider.BuildAssembly(code, null);
            Assert.IsNotNull(actual);
        }

		[Test()]
        public void ShouldBeAbleToInstantiateTheAssembly()
        {
            string code = assemblyTest1;
            Assembly actual = null;
            actual = assemblyProvider.BuildAssembly(code, null);
            string name = actual.FullName;
            Object instance = actual.CreateInstance("SharpWeld.ClassProvider.SampleClass");

            Assert.IsNotNull(instance);
            MethodInfo method = instance.GetType().GetMethod("GetCrazyString");
            string retVal = method.Invoke(instance, new Object[0]) as string;
            Assert.AreEqual("One Crazy String", retVal);
        }

    }


}
