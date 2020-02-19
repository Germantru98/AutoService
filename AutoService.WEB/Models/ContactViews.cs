using System.Collections.Generic;

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
    }
}