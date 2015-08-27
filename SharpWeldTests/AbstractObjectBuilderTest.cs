using SharpWeld.ClassProvider;
using SharpWeldTest.Mocks;
using NUnit.Framework;
using System;
using System.Reflection;
using SharpWeld;
using System.Security.Cryptography;
using SharpWeld.Attributes;
using SharpWeldTest.CustomAttributeMocks;

namespace SharpWeldTest
{
    
    
    /// <summary>
    ///This is a test class for AbstractObjectBuilderTest and is intended
    ///to contain all AbstractObjectBuilderTest Unit Tests
    ///</summary>
	[TestFixture()]
    public class AbstractObjectBuilderTest
    {

        private Builder value;
        private Type type;

		[SetUp()]
        public void MyTestInitialize()
        {
            value = new AssemblyBuilder(new CSharpProvider());
            type = typeof(AbstractMockObject);
        }
        
        //Use TestCleanup to run code after each test has run
		[TearDown()]
        public void MyTestCleanup()
        {
            value = null;
            type = null;
        }

        /// <summary>
        ///A test for Construct
        ///</summary>
        public void ConstructTestHelper<T>()
        {
            AbstractObjectBuilder<T> target = new AbstractObjectBuilder<T>(value);
            T actual;
            actual = target.Construct(type, null);
            Assert.IsNotNull(actual);
        }

        [Test()]
        public void ConstructTest()
        {
            ConstructTestHelper<AbstractMockObject>();
        }

        /// <summary>
        ///A test for Construct
        ///</summary>
        public T ConstructDiscreteHelper<T>()
        {
            AbstractObjectBuilder<T> target = new AbstractObjectBuilder<T>(value);
            T actual;
            actual = target.Construct(type, null);
            Assert.IsNotNull(actual);
            return actual;
        }

        [Test()]
        public void ShouldConstructWithDiscretPropertiesIfNeeded()
        {
            AbstractMockObject mock = ConstructDiscreteHelper<AbstractMockObject>();
            PropertyInfo property = mock.GetType().GetProperty("__LastName");
            mock.LastName = "sampleValue";
            Assert.AreEqual("sampleValue", property.GetGetMethod().Invoke(mock, null));

            property = mock.GetType().GetProperty("__MiddleName");
            property.GetSetMethod().Invoke(mock, new Object[1] { "middleName" });
            Assert.AreEqual("middleName", mock.MiddleName);

            property = mock.GetType().GetProperty("__FirstName");
            Assert.IsNull(property);
        }

        [Test()]
        public void ShouldConstructAndOverrideAbstractMethods()
        {
            AbstractMockObject mock = ConstructDiscreteHelper<AbstractMockObject>();
            try
            {
                mock.FindEmployer("test");
            }
            catch (Exception) { }
        }

		[Test()]
        public void ShouldBeAbleToCallAbstractProperties()
        {
            AbstractMockObject mock = ConstructDiscreteHelper<AbstractMockObject>();
            PropertyInfo property = mock.GetType().GetProperty("__MiddleName");
            property.GetSetMethod().Invoke(mock, new Object[1] { "middleName" });

            Assert.AreEqual("middleName", mock.GetMiddleName());
        }

		[Test()]
        public void ShouldBeAbleToCallAbstractMethodsAndBeImplementedByAnAttribute()
        {
            AbstractMockObject mock = ConstructDiscreteHelper<AbstractMockObject>();
            Decorator decorator = new ObjectDecorator(new ObjectInstantiator());
            decorator.Decorate<AbstractMockObject>(mock);
            Assert.AreEqual("The Next Employer", mock.TestGetEmployer());
            Assert.AreEqual("MyName 1", mock.GetTitleAndPosition("MyName", 1));
            Assert.AreEqual((-3 - 5 - 7 - 2), mock.Subtract(3, 5, 7, 2));
            Assert.AreEqual((3 + 5 + 7 + 2), mock.Add(3, 5, 7, 2));
            Assert.AreEqual(0, mock.GetZero());
        }

        //Unsupported as of right now.
        //[TestMethod()]
        //public void ShouldParseAttributeConstructorArgs()
        //{
        //    AbstractObjectBuilder<MockInjectable> builder = new AbstractObjectBuilder<MockInjectable>(new AssemblyBuilder(new CSharpProvider()));
        //    MockInjectable mock = builder.Construct(typeof(MockInjectable), null);
        //    Decorator decorator = new ObjectDecorator(new ObjectInstantiator());
        //    decorator.Decorate<MockInjectable>(mock);

        //    Assert.AreEqual("somewateryvalue", mock.MoreWater);
        //}

    }

}
