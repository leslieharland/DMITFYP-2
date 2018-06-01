using System;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;
using Microsoft.Extensions.Primitives;

namespace WebApp.ModelBinders
{
    public class ContactModelBinder 
    {
        //public override object BindModel(ControllerContext controllerContext,
        //                                     ModelBindingContext bindingContext)
        //{
        //    var request = controllerContext.HttpContext.Request;

        //    var result = new StringValues();
        //    request.Form.TryGetValue("btnExport", out result);
        //    return new ContactViewModel
        //    {
        //        Export = ""             
        //    };
        //}
    }
}