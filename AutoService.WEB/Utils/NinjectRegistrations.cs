using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject.Modules;
using AutoService.BLL.Services;
using AutoService.BLL.Interfaces;

namespace AutoService.WEB.Utils
{
    public class NinjectRegistrations:NinjectModule
    {
        public override void Load()
        {
            Bind<IContactData>().To<ContactDataService>();
        }
    }
}