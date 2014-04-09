﻿using System;
using System.Collections.Generic;
using System.Data.Spatial;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;

using IMvcModelBinder = System.Web.Mvc.IModelBinder;
using IWebFormsModelBinder = System.Web.ModelBinding.IModelBinder;

using MvcModelBindingContext = System.Web.Mvc.ModelBindingContext;
using WebFormsModelBindingContext = System.Web.ModelBinding.ModelBindingContext;

namespace MyFestival
{
    public class DbGeographyModelBinder : IMvcModelBinder, IWebFormsModelBinder
    {
        public object BindModel(ControllerContext controllerContext, MvcModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            return BindModelImpl(valueProviderResult != null ? valueProviderResult.AttemptedValue : null);
        }

        public bool BindModel(ModelBindingExecutionContext modelBindingExecutionContext, WebFormsModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            bindingContext.Model = BindModelImpl(valueProviderResult != null ? valueProviderResult.AttemptedValue : null);
            return bindingContext.Model != null;
        }

        private DbGeography BindModelImpl(string value)
        {
            if (value == null)
            {
                return (DbGeography)null;
            }
            string[] latLongStr = value.Split(',');
            // TODO: More error handling here, what if there is more than 2 pieces or less than 2?
            //       Are we supposed to populate ModelState with errors here if we can't conver the value to a point?
            string point = string.Format("POINT ({0} {1})", latLongStr[1], latLongStr[0]);
            //4326 format puts LONGITUDE first then LATITUDE
            DbGeography result = DbGeography.FromText(point, 4326);
            return result;
        }
    }



    public class EFModelBinderProviderMvc : System.Web.Mvc.IModelBinderProvider
    {
        public IMvcModelBinder GetBinder(Type modelType)
        {
            if (modelType == typeof(DbGeography))
                return new DbGeographyModelBinder();
            return null;
        }
    }

    public class EFModelBinderProviderWebForms : System.Web.ModelBinding.ModelBinderProvider
    {
        public override IWebFormsModelBinder GetBinder(ModelBindingExecutionContext modelBindingExecutionContext, WebFormsModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType == typeof(DbGeography))
                return new DbGeographyModelBinder();
            return null;
        }
    }
}