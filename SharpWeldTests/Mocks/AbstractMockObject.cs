using System.Linq;
using System.Text;
using System;
using SharpWeld.Attributes;
using System.Reflection;
using SharpWeldTests.Mocks;

namespace SharpWeldTest.Mocks
{
    public abstract class AbstractMockObject
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
    }


}
