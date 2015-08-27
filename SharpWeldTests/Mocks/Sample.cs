using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpWeld.Utilities
{
    public abstract class Sample
    {

        public virtual string OtherMagic(System.String arg1, string arg2, string arg3, string blarg) {
            return _OtherMagic(arg1, arg2, arg3, blarg);
        }

        private System.Func<System.String, string, string, string, string> _OtherMagic;

        public void __OtherMagic(System.Func<System.String, string, string, string, string> value)
        { 
            _OtherMagic = value; 
        }

        public abstract string FirstName { get; set; }

        public abstract string LastName { set; }

        public abstract string MiddleName { get; }

        public abstract string AbstractMethod();
    }

    public class SampleExt : Sample
    {

        private string _firstname;

        public override string FirstName
        {
            get
            {
                return _firstname;
            }
            set
            {
                _firstname = value;
            }
        }

        public override string LastName
        {
            set {  }
        }

        public override string MiddleName
        {
            get { return ""; }
        }
        public override string AbstractMethod()
        {
            return "";
        }
    }
}
