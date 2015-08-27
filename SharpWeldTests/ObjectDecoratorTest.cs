using SharpWeld;
using NUnit.Framework;
using System;
using SharpWeldTest.Mocks;
using System.Collections.Generic;
using SharpWeld.Attributes;
using System.Collections;
using SharpWeldTest.CustomAttributeMocks;

namespace SharpWeldTest
{
    
    
    /// <summary>
    ///This is a test class for ObjectDecoratorTest and is intended
    ///to contain all ObjectDecoratorTest Unit Tests
    ///</summary>
	[TestFixture()]
    public class ObjectDecoratorTest
    {
        private static TestableObjectInstantiator inst;
        private ObjectDecorator decorator;

		[TestFixtureSetUp()]
        public void MyClassInitialize()
        {
            inst = new TestableObjectInstantiator();
        }

		[TestFixtureTearDown()]
        public void MyTestFixtureTearDown()
        {
			inst = null;
        }

		[SetUp()]
		public void MyTestSetup()
		{
			decorator = new ObjectDecorator(inst);
		}
        
        //Use TestCleanup to run code after each test has run
		[TearDown()]
        public void MyTestCleanup()
        {
            inst.Clear();
            decorator = null;
        }


        /// <summary>
        ///A test for InitializeType
        ///</summary>
		[Test()]
        public void ShouldInitializeType()
        {
            MockObject obj = new MockObject();
            inst.obj = obj;

            MockObject actual = decorator.InitializeType<MockObject>(typeof(MockObject), new Object[0]);

            Assert.IsTrue(actual.AttibuteClass is MockClassAttribute);
            MockClassAttribute mockClassAttribute = (MockClassAttribute)actual.AttibuteClass;
            Assert.AreEqual(0, mockClassAttribute.preInstantiated);
            Assert.AreEqual(1, mockClassAttribute.before);
            Assert.AreEqual(2, mockClassAttribute.postInstantiated);
            Assert.AreEqual(3, mockClassAttribute.after);
        }

		[Test()]
        public void ShouldInitializeTypeWithSeveralAttributes()
        {
            MockObjectMultipleAttributes obj = new MockObjectMultipleAttributes();
            inst.obj = obj;

            MockObjectMultipleAttributes actual = decorator.InitializeType<MockObjectMultipleAttributes>(typeof(MockObjectMultipleAttributes), new Object[0]);

            for (int i = actual.AttibuteClasses.Count - 1; i >= 0; i--)
            {
                ClassAttribute attribute = (ClassAttribute) actual.AttibuteClasses[i];
                Assert.AreEqual(0, ((MockAttribute)attribute).preInstantiated);
                Assert.AreEqual(1, ((MockAttribute)attribute).before);
                Assert.AreEqual(2, ((MockAttribute)attribute).postInstantiated);
                Assert.AreEqual(3, ((MockAttribute)attribute).after);
            }
        }

		[Test()]
        public void ShouldDecoratePropertiesOnInstantiatedObjects()
        {
            inst.obj = new MockPropertyAttribute();

            MockObject obj = new MockObject();
            decorator.Decorate(obj);

			SharpWeld.Attributes.PropertyAttribute pa = obj.AttributeProperty;
            Assert.IsNotNull(((MockPropertyAttribute)pa).Obj);
            Assert.IsNotNull(((MockPropertyAttribute)pa).PropertyInfo);
            Assert.AreEqual("DecoratedString", ((MockPropertyAttribute)pa).PropertyInfo.Name);
        }
        
		[Test()]
        public void ShouldDecorateMethodsOnInstantiatedObjects()
        {
            inst.obj = new MockMethod();

            MockObject2 obj = new MockObject2();
            decorator.Decorate(obj);

            MethodAttribute pa = obj.AttributeMethod;
            Assert.IsNotNull(((MockMethod)pa).Obj);
            Assert.IsNotNull(((MockMethod)pa).MethodInformation);
            Assert.AreEqual("DecoratedMethod", ((MockMethod)pa).MethodInformation.Name);
        }

		[Test()]
        public void ShouldInjectObjectDecoratorIntoClassAttributes()
        {
            MockObject obj = new MockObject();
            inst.obj = obj;

            MockObject actual = decorator.InitializeType<MockObject>(typeof(MockObject), new Object[0]);

            Assert.IsTrue(actual.AttibuteClass is MockClassAttribute);
            MockClassAttribute mockClassAttribute = (MockClassAttribute)actual.AttibuteClass;
            Assert.AreEqual(decorator, mockClassAttribute.GetInjectedDecorator());
        }

		[Test()]
        public void ShouldInjectObjectDecoratorIntoInstanceAttributes()
        {
            inst.obj = new MockMethod();

            MockObject2 obj2 = new MockObject2();
            decorator.Decorate(obj2);

            MockMethod pa = (MockMethod) obj2.AttributeMethod;
            Assert.AreEqual(decorator, pa.GetInjectedDecorator());

            inst.obj = new MockPropertyAttribute();

            MockObject obj = new MockObject();
            decorator.Decorate(obj);

            MockPropertyAttribute ap = (MockPropertyAttribute) obj.AttributeProperty;
            Assert.AreEqual(decorator, ap.GetInjectedDecorator());
        }

    }

    sealed class TestableObjectInstantiator : ObjectInstantiator
    {
        private Stack<Object> objects = new Stack<Object>();

        public Type type { get; set; }
        public Object obj { 
            get 
            {
                return objects.Pop();
            }
            set
            {
                objects.Push(value);
            }
        }

        public override Type GetTypeByName(string name)
        {
            return type;
        }

        public override T GetInstanceByName<T>(string name, object[] args)
        {
            return (T)obj;
        }

        public override T GetInstanceByType<T>(Type type, object[] args)
        {
            return (T)obj;
        }

        public void Clear()
        {
            objects.Clear();
        }
    }
}
