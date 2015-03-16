using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

using Disposable.Common;

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