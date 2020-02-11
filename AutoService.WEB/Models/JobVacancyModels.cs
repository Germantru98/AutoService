using System.ComponentModel.DataAnnotations;

namespace AutoService.WEB.Models
{
    public class JobVacancy
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название вакансии не может быть пустым полем")]
        public string VacancyName { get; set; }

        [Required(ErrorMessage = "Описание вакансии не может быть пустым полем")]
        public string VacancyDescription { get; set; }

        [Required(ErrorMessage = "Зарплата не может быть пустым полем")]
        public string Salary { get; set; }

        [Required(ErrorMessage = "Почта вакансии не может быть пустым полем")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Контактный телефон вакансии не может быть пустым полем")]
        public string ContactPhone { get; set; }
    }
}