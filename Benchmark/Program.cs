using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpWeld.ClassProvider;
using SharpWeld;

namespace Benchmarking
{
    class Program
    {
        private const int ITERATIONS = 100000;

        public static void Main()
        {
            DateTime startTime = DateTime.Now;
            for (int i = 0; i < ITERATIONS; i++)
            {
                Method1ToBenchMark();
            }
            DateTime endTime = DateTime.Now;

            System.Console.WriteLine(ITERATIONS + " Iterations");
            System.Console.WriteLine("Time = " + (endTime - startTime) + "ms");

			startTime = DateTime.Now;
			for (int i = 0; i < ITERATIONS; i++)
			{
				Method2ToBenchMark();
			}
			endTime = DateTime.Now;

			System.Console.WriteLine(ITERATIONS + " Iterations");
			System.Console.WriteLine("Time = " + (endTime - startTime) + "ms");

            string read = System.Console.ReadLine();
        }

        private static void Method1ToBenchMark()
        {
            AbstractObjectBuilder<MockClass> builder = new AbstractObjectBuilder<MockClass>(new AssemblyBuilder(new CSharpProvider()));
            MockClass mock = builder.Construct(typeof(MockClass), null);
            Decorator decorator = new ObjectDecorator(new ObjectInstantiator());
            decorator.Decorate<MockClass>(mock);

            double value = mock.GetZero();
        }

        private static void Method2ToBenchMark()
        {
            AssemblyOpCodeBuilder builder = new AssemblyOpCodeBuilder();
            MockClass mock = builder.BuildAssemblyFromType<MockClass>(typeof(MockClass), null);
            Decorator decorator = new ObjectDecorator(new ObjectInstantiator());
            decorator.Decorate<MockClass>(mock);

            double value = mock.GetZero();
        }
    }
}
