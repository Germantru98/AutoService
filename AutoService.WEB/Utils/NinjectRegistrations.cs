using AutoService.BLL.Interfaces;
using AutoService.BLL.Services;
using Ninject.Modules;

namespace AutoService.WEB.Utils
{
    public class NinjectRegistrations : NinjectModule
    {
        public override void Load()
        {
            Bind<IContactData>().To<ContactDataService>();
            Bind<IUserService>().To<UserService>();
        }
    }
}