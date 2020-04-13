using AutoService.WEB.Models;
using AutoService.WEB.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoService.WEB.Utils
{
    public class CarLogic : ICarLogic
    {
        public CarView MapCarToCarView(Car car)
        {
            return new CarView(car.Model, car.Color, car.Year, car.CarImageHref);
        }
    }
}