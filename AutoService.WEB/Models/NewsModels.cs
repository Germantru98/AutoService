using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutoService.WEB.Models
{
    public class News
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string NewsText { get; set; }
        public DateTime DateOfCreation { get; set; }

        public News()
        {

        }
        public News(int id, string title, string newsText)
        {
            Id = id;
            Title = title;
            NewsText = newsText;
            DateOfCreation = DateTime.Today;
        }
        public News(string title, string newsText)
        {
            Title = title;
            NewsText = newsText;
            DateOfCreation = DateOfCreation = DateTime.Today;
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
        public CreateNews()
        {

        }
        public CreateNews(string title, string newsText)
        {
            Title = title;
            NewsText = newsText;
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