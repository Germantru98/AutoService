using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AutoService.WEB.Models
{
    public class News
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }
        public string NewsText { get; set; }
        public string SlideTitle { get; set; }
        public string SlideDescription { get; set; }
        public string ImgHref { get; set; }
        public DateTime DateOfCreation { get; set; }

        public News()
        {
        }
    }

    public class NewsView
    {
        public string Title { get; set; }
        public string NewsText { get; set; }
        public string DateOfCreation { get; set; }
        public string SlideImg { get; set; }

        public NewsView()
        {
        }

        public NewsView(string title, string newsText, string dateOfCreation, string slideImg)
        {
            Title = title;
            NewsText = newsText;
            DateOfCreation = dateOfCreation;
            SlideImg = slideImg;
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
        [Display(Name = "Ссылка на изображение слайда")]
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

        [Required]
        [Display(Name = "Заголовок слайда")]
        public string SlideTitle { get; set; }

        [Required]
        [Display(Name = "Описание слайда")]
        public string SlideDescription { get; set; }

        [Required]
        [Url]
        [Display(Name = "Ссылка на изображение слайда")]
        public string ImgHref { get; set; }

        public EditNews()
        {
        }
    }

    public class Slide
    {
        public int NewsId { get; set; }
        public string SlideTitle { get; set; }
        public string SlideDescription { get; set; }
        public string ImgHref { get; set; }

        public Slide()
        {
        }

        public Slide(int newsId, string slideTitle, string slideDescription, string imgHref)
        {
            NewsId = newsId;
            SlideTitle = slideTitle;
            SlideDescription = slideDescription;
            ImgHref = imgHref;
        }
    }
}