using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dilemma.Web.Extensions
{
    public static class HtmlExtensions
    {
        public static SelectList SelectListFrom<TType>(           
            this HtmlHelper helper,
            IEnumerable<TType> objects,
            Func<TType, object> keyFunc,
            Func<TType, object> valueFunc,
            object selectedValue)
        {
            return null;
        }
    }
}