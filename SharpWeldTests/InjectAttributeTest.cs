using SharpWeld.CustomAttributes;
using NUnit.Framework;
using System;
using System.Reflection;
using SharpWeld.ClassProvider;
using SharpWeldTest.CustomAttributeMocks;
using SharpWeld;
using SharpWeldTest.Mocks;

namespace SharpWeldTest
{
    
    
    /// <summary>
    ///This is a test class for InjectAttributeTest and is intended
    ///to contain all InjectAttributeTest Unit Tests
    ///</summary>
	[TestFixture()]
    public class InjectAttributeTest
    {


        /// <summary>
        ///A test for DecorateProperty
        ///</summary>
		[Test()]
        public void DecoratePropertyTest()
        {
            AbstractObjectBuilder<MockInjectable> builder = new AbstractObjectBuilder<MockInjectable>(new AssemblyBuilder(new CSharpProvider()));
            MockInjectable mock = builder.Construct(typeof(MockInjectable), null);
            Decorator decorator = new ObjectDecorator(new ObjectInstantiator());
            decorator.Decorate<MockInjectable>(mock);

            Assert.IsNotNull(mock.Fire);
            Assert.IsTrue(mock.Fire is Fire);

            Assert.IsNotNull(mock.Water);
            Assert.IsTrue(mock.Water is Water);
        }

    }
}
