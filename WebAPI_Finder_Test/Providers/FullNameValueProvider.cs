using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Http.ValueProviders;

namespace WebAPI_Finder_Test.Providers
{
    public class FullNameValueProvider : IValueProvider
    {
        public bool ContainsPrefix(string prefix)
        {
            return string.Compare("fullname", prefix, true) == 0;
        }

        public ValueProviderResult GetValue(string key)
        {
            var value = HttpContext.Current.Request.QueryString["fullname"];
            if (String.IsNullOrEmpty(value))
            {
                return null;
            }

            return ContainsPrefix(key) ? new ValueProviderResult(value, null,
            CultureInfo.InvariantCulture) : null;
        }
    }
}