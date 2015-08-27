using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpWeld.Attributes
{
    public interface AttributeDecorator
    {
        /// <summary>
        /// This is the first function that should be called after the attribute has
        /// been created.
        /// </summary>
        /// <param name="obj">The object the attribute is decorating</param>
        void First(Object obj);

        /// <summary>
        /// This is the last function that should be called when all other details of the
        /// attribute have been invoked.
        /// </summary>
        /// <param name="obj">The object the attribute is decorating</param>
        void Last(Object obj);
    }
}
