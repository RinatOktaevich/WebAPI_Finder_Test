using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;

namespace WebAPI_Finder_Test.Providers
{
    public class FilterModelBinder : IModelBinder
    {
        public FilterModelBinder()
        {

        }

        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            bool IsTrue = false;
            Filters filter = null;

            if (bindingContext.ModelType != typeof(Filters))
            {
                return false;
            }

            //Берём город
            ValueProviderResult val = bindingContext.ValueProvider.GetValue(
                "cityid");
            if (val != null)
            {
                int key = Convert.ToInt16(val.RawValue);
                if (key == 0)
                {
                    bindingContext.ModelState.AddModelError(
                        bindingContext.ModelName, "Wrong value type");
                    return false;
                }

                filter = new CityFilter(key);
                IsTrue = true;
            }


            //Берём возраст
            ValueProviderResult valMin = bindingContext.ValueProvider.GetValue(
                "minAge");
            ValueProviderResult valMax = bindingContext.ValueProvider.GetValue(
               "maxAge");

            if (valMin != null || valMax != null)
            {
                int minAge = 0;
                int maxAge = 100;

                if (valMin != null)
                    minAge = Convert.ToInt16(valMin.RawValue);

                if (valMax != null)
                    maxAge = Convert.ToInt16(valMax.RawValue);

                if (filter != null)
                    filter.Chain = new AgeFilter(minAge, maxAge);
                else
                    filter = new AgeFilter(minAge, maxAge);
                IsTrue = true;
            }













            if (IsTrue)
                bindingContext.Model = filter;

            return IsTrue;
        }


    }
}