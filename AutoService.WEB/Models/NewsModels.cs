using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;


namespace AutoService.WEB.Models
{
    public class News
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string NewsText { get; set; }
        public DateTime DateOfCreation { get; set; }
        public virtual HomeMainCarouselItem CarouselItem { get; set; }

        public News()
        {
        }
    }

    public class NewsView
    {
        public string Title { get; set; }
        public string NewsText { get; set; }
        public string DateOfCreation { get; set; }

        public NewsView()
        {
        }

        public NewsView(string title, string newsText, string dateOfCreation)
        {
            Title = title;
            NewsText = newsText;
            DateOfCreation = dateOfCreation;
        }
    }

    public class CreateNews
    {
        [Required]
        [Display(Name = "Заголовок")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Текст новости")]
        public string NewsText { get; set; }
        [Required]
        [Display(Name = "Заголовок слайда")]
        public string SlideTitle { get; set; }
        [Required]
        [Display(Name = "Описание слайда")]
        public string SlideDescription { get; set; }
        [Required]
        [Url]
        [Display(Name = "Ссылка слайда")]
        public string ImgHref { get; set; }

        public CreateNews()
        {
        }
      
    }

    public class EditNews
    {
        [HiddenInput]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Заголовок")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Текст новости")]
        public string NewsText { get; set; }

        public EditNews()
        {
        }

        public EditNews(int id, string title, string newsText)
        {
            Id = id;
            Title = title;
            NewsText = newsText;
        }
    }
}