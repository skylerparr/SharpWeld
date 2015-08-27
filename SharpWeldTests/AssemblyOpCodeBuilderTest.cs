using SharpWeld.ClassProvider;
using NUnit.Framework;
using System;
using System.Reflection;
using SharpWeldTest.Mocks;
using SharpWeld.Attributes;
using SharpWeld;
using SharpWeldTest.CustomAttributeMocks;

namespace SharpWeldTest
{
    
    
    /// <summary>
    ///This is a test class for AssemblyOpCodeBuilderTest and is intended
    ///to contain all AssemblyOpCodeBuilderTest Unit Tests
    ///</summary>
    [TestFixture()]
    public class AssemblyOpCodeBuilderTest
    {


        private AssemblyOpCodeBuilder builder;

		[SetUp()]
        public void MyTestInitialize()
        {
            builder = new AssemblyOpCodeBuilder(); 

        }
        
        //Use TestCleanup to run code after each test has run
		[TearDown()]
        public void MyTestCleanup()
        {
            builder = null;
        }
        
        /// <summary>
        ///A test for BuildAssemblyFromType
        ///</summary>
        [Test()]
        public void ShouldBuildInstanceFromExistingConcreteType()
        {
            MockObject actual = builder.BuildAssemblyFromType<MockObject>(typeof(MockObject), null);
            Assert.IsNotNull(actual);
        }

		[Test()]
        public void ShouldBuildAbstractTypeWithAttributes()
        {
            MockClass actual = builder.BuildAssemblyFromType<MockClass>(typeof(MockClass), null);
            Object[] attributes = actual.GetType().GetMethod("DoSomeMagic").GetCustomAttributes(typeof(MethodAttribute), true);
            Assert.AreEqual(1, attributes.Length);
            Assert.IsTrue(attributes[0] is MockMethodAbstractAttribute);
        }

		[Test()]
        public void ShouldBuildAbstractTypeThenGetDecorated()
        {
            MockClass1 actual = builder.BuildAssemblyFromType<MockClass1>(typeof(MockClass1), null);
            Decorator decorator = new ObjectDecorator(new ObjectInstantiator());
            decorator.Decorate<MockClass1>(actual);
            Assert.AreEqual("This is a sentence", actual.OtherMagic());
        }

		[Test()]
        public void ShouldBuildAbstractTypeThenGetDecoratedWith1Arg()
        {
            MockClass2 actual = builder.BuildAssemblyFromType<MockClass2>(typeof(MockClass2), null);
            Decorator decorator = new ObjectDecorator(new ObjectInstantiator());
            decorator.Decorate<MockClass2>(actual);
            Assert.AreEqual("string", actual.OtherMagic("string"));
        }

		[Test()]
        public void ShouldBuildAbstractTypeThenGetDecoratedWith2Args()
        {
            MockClass3 actual = builder.BuildAssemblyFromType<MockClass3>(typeof(MockClass3), null);
            Decorator decorator = new ObjectDecorator(new ObjectInstantiator());
            decorator.Decorate<MockClass3>(actual);
            Assert.AreEqual("some1", actual.OtherMagic("some", 1));
        }

		[Test()]
        public void ShouldBuildAbstractTypeThenGetDecoratedWith3Args()
        {
            MockClass actual = builder.BuildAssemblyFromType<MockClass>(typeof(MockClass), null);
            Decorator decorator = new ObjectDecorator(new ObjectInstantiator());
            decorator.Decorate<MockClass>(actual);
            Assert.AreEqual("some1help", actual.DoSomeMagic("some", 1, "help"));
        }

		[Test()]
        public void ShouldBuildAbstractTypeThenGetDecoratedWith4Args()
        {
            MockClass4 actual = builder.BuildAssemblyFromType<MockClass4>(typeof(MockClass4), null);
            Decorator decorator = new ObjectDecorator(new ObjectInstantiator());
            decorator.Decorate<MockClass4>(actual);
            Assert.AreEqual("some1helpme", actual.DoSomeMagic("some", 1, "help", "me"));
        }

		[Test()]
        public void ShouldBuildAbstractTypeAndGenerateGetterAndSetter()
        {
            AbstractMockObject actual = builder.BuildAssemblyFromType<AbstractMockObject>(typeof(AbstractMockObject), null);
            actual.FirstName = "firstname";
            Assert.AreEqual("firstname", actual.FirstName);
        }

		[Test()]
        public void ShouldImplementPropertiesAndDecorateThem()
        {
            MockInjectable actual = builder.BuildAssemblyFromType<MockInjectable>(typeof(MockInjectable), null);
            Decorator decorator = new ObjectDecorator(new ObjectInstantiator());
            decorator.Decorate<MockInjectable>(actual);
            Assert.IsNotNull(actual.Earth);
        }
    }
}
