using System.ComponentModel.DataAnnotations;

namespace AutoService.WEB.Models
{
    public class JobVacancy
    {
        public int Id { get; set; }
        public string VacancyName { get; set; }
        public string VacancyDescription { get; set; }
        public string Salary { get; set; }
        public string Email { get; set; }
        public string ContactPhone { get; set; }
    }
}