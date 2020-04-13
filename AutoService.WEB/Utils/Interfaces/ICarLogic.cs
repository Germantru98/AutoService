using AutoService.WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.WEB.Utils.Interfaces
{
    public interface ICarLogic
    {
        CarView MapCarToCarView(Car car);
    }
}
