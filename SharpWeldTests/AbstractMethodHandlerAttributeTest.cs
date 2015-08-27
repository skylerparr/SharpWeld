using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using SharpWeld.ClassProvider;
using SharpWeld;
using SharpWeldTest.Mocks;
using NUnit.Framework;

namespace SharpWeldTest
{
	[TestFixture()]
    public class AbstractMethodHandlerAttributeTest
    {
		[Test()]
        public void ShouldInterruptTheAbstractMethodAndUseAttributeFunction()
        {
            AbstractObjectBuilder<MockClass> builder = new AbstractObjectBuilder<MockClass>(new AssemblyBuilder(new CSharpProvider()));
            MockClass mock = builder.Construct(typeof(MockClass), null);
            Decorator decorator = new ObjectDecorator(new ObjectInstantiator());
            decorator.Decorate<MockClass>(mock);

            Assert.AreEqual("some1helpme", mock.DoSomeMagic("some", 1, "helpme"));
        }
    }


}
