using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoService.DAL.Entities
{
    public class ClientProfile
    {
        [Key]
        [ForeignKey("ApplicationUser")]
        public int Id { get; set; }

        public IEnumerable<Car> ClientCars { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}