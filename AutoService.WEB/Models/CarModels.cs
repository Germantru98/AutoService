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
}