using AutoService.WEB.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoService.WEB.Utils.Interfaces
{
    public interface INewsPageLogic : IDisposable
    {
        Task AddNews(CreateNews news);

        Task RemoveNews(int? newsId);

        Task EditNews(EditNews editedNews);

        Task<List<News>> GetAllNews();

        Task<NewsView> GetNews(int? newsId);

        Task<EditNews> StartEditNews(int? newsId);
    }
}