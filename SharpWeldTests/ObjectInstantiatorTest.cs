using SharpWeld;
using NUnit.Framework;
using System;
using SharpWeld.Attributes;
using SharpWeldTest.Mocks;

namespace SharpWeldTest
{
    
    
    /// <summary>
    ///This is a test class for ObjectInstantiatorTest and is intended
    ///to contain all ObjectInstantiatorTest Unit Tests
    ///</summary>
	[TestFixture()]
    public class ObjectInstantiatorTest
    {

        private static ObjectInstantiator target;

		[TestFixtureSetUp()]
        public void MyClassInitialize()
        {
            target = new ObjectInstantiator();
        }
        //
        //Use ClassCleanup to run code after all tests in a class have run
		[TestFixtureTearDown()]
        public void MyClassCleanup()
        {
            target = null;
        }

        /// <summary>
        ///A test for GetTypeByName
        ///</summary>
        [Test()]
        public void GetTypeByNameTest()
        {
			string name = String.Format("SharpWeldTest.Mocks.MockObject,{0}", GetAssemblyName());
            Type expected = typeof(MockObject); 
            Type actual;
            actual = target.GetTypeByName(name);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetInstanceByType
        ///</summary>
        public void GetInstanceByTypeTestHelper<T>()
        {
            Type type = typeof(MockObject);
            object[] args = null;
            T actual;
            actual = target.GetInstanceByType<T>(type, args);
            Assert.IsNotNull(actual);
        }

        [Test()]
        public void GetInstanceByTypeTest()
        {
            GetInstanceByTypeTestHelper<MockObject>();
        }

        /// <summary>
        ///A test for GetInstanceByName
        ///</summary>
        public void GetInstanceByNameTestHelper<T>()
        {
			string name = String.Format("SharpWeldTest.Mocks.MockObject,{0}", GetAssemblyName());
            object[] args = null;
            T actual;
            actual = target.GetInstanceByName<T>(name, args);
            Assert.IsNotNull(actual);
        }

		[Test()]
        public void GetInstanceByNameTest()
        {
            GetInstanceByNameTestHelper<MockObject>();
        }

		[Test()]
        public void ShouldCreateObjectWithConstructor()
        {
            Type type = typeof(MockObject);
            object[] args = new Object[2] { "argument1", "argument2" };
            MockObject actual;
            actual = target.GetInstanceByType<MockObject>(type, args);
            Assert.AreEqual(args[0], actual.Arg1);
            Assert.AreEqual(args[1], actual.Arg2);

        }

		private string GetAssemblyName()
		{
			return System.Reflection.Assembly.GetExecutingAssembly ().GetName ().Name;
		}
    }

}
