using System.ComponentModel.DataAnnotations;

namespace AutoService.WEB.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public string Year { get; set; }
        public string CarImageHref { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }

    public class CarsStorageItem
    {
        public int Id { get; set; }
        public int? CarId { get; set; }
        public Car Car { get; set; }
        public string UserId { get; set; }
    }

    public class CarView
    {
        public int CarId { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public string Year { get; set; }
        public string CarImageHref { get; set; }
        public string FullName { get; set; }

        public CarView(int id, string model, string color, string year, string carImageHref)
        {
            CarId = id;
            Model = model;
            Color = color;
            Year = year;
            CarImageHref = carImageHref;
            FullName = ToString();
        }

        public override string ToString()
        {
            return $"Модель: {Model} Цвет: {Color} Год выпуска: {Year}";
        }
    }

    public class EditCarView
    {
        public int CarId { get; set; }

        [Display(Name = "Модель автомобиля")]
        public string Model { get; set; }

        [Display(Name = "Цвет автомобиля")]
        public string Color { get; set; }

        [Display(Name = "Год выпуска автомобиля")]
        public string Year { get; set; }

        [Display(Name = "Ссылка на изображение авто")]
        public string CarImgHref { get; set; }

        public EditCarView()
        {
        }

        public EditCarView(int carId, string model, string color, string year, string carImgHref)
        {
            CarId = carId;
            Model = model;
            Color = color;
            Year = year;
            CarImgHref = carImgHref;
        }
    }

    public class AddNewCarViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Модель автомобиля")]
        public string CarModel { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Цвет автомобиля")]
        public string CarColor { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Год выпуска автомобиля")]
        public string CarYear { get; set; }

        [Display(Name = "Ссылка на изображение авто")]
        public string CarImgHref { get; set; }
    }

    public class DeleteCarView
    {
        public int CarId { get; set; }
        public string UserId { get; set; }

        public DeleteCarView()
        {
        }

        public DeleteCarView(int carId, string userId)
        {
            CarId = carId;
            UserId = userId;
        }
    }
}