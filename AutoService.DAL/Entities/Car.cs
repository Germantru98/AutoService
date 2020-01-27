using System;

namespace AutoService.DAL.Entities
{
    public class Car
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public DateTime year { get; set; }

        public int? ClientProfileId { get; set; }
        public ClientProfile User { get; set; }
    }
}