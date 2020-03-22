using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AutoService.WEB.Models
{
    public class ContactView
    {
        public string YandexFrameSrc { get; set; }
        public string Address { get; set; }
        public string InstagramHref { get; set; }
        public string FacebookHref { get; set; }
        public string VKHref { get; set; }
        public string VKImageHref { get; set; }
        public string FacebookImageHref { get; set; }
        public string InstagramImageHref { get; set; }
        public List<string> ContactPhones { get; set; }
        public List<string> ContactEmails { get; set; }
    }

    public class ContactItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public bool isActive { get; set; }

        public ContactItem()
        {
        }

        public ContactItem(string name, string value, bool status)
        {
            Name = name;
            Value = value;
            isActive = status;
        }
    }

    public class AddNewContactView
    {
        [Required(ErrorMessage = "Ошибка, не указан тип информации")]
        [Display(Name = "Наименование данных")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Ошибка, данное поле не может быть пустым")]
        [Display(Name = "Поле данных")]
        public string Value { get; set; }
    }

    public class EditContactInformationView
    {
        public List<ContactItem> ContactItems { get; set; }
        public AddNewContactView AddNewContactView { get; set; }
    }

    public class ContactItemView
    {
        [Display(Name = "Тип данных")]
        public string Type { get; set; }

        [Display(Name = "Данные")]
        public string Value { get; set; }

        public ContactItemView()
        {
        }

        public ContactItemView(string type, string value)
        {
            Type = type;
            Value = value;
        }
    }
}