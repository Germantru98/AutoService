using AutoService.WEB.Models;
using AutoService.WEB.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AutoService.WEB.Utils
{
    public class NewsPageLogic : INewsPageLogic
    {
        ApplicationDbContext _db;

        public NewsPageLogic(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task AddNews(CreateNews news)
        {
            var item = new News(news.Title, news.NewsText);
            _db.News.Add(item);
            await _db.SaveChangesAsync();
        }

        public async Task EditNews(EditNews editedNews)
        {
            var news = await _db.News.FindAsync(editedNews.Id);
            news.Title = editedNews.Title;
            news.NewsText = editedNews.NewsText;
            news.DateOfCreation = DateTime.Today;
            _db.Entry(news).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task<List<News>> GetAllNews()
        {
            var result = await _db.News.ToListAsync();
            return result;
        }

        public async Task RemoveNews(int? newsId)
        {
            if (newsId == null)
            {
                throw new ArgumentNullException();
            }
            var removedNews = await _db.News.FindAsync(newsId);
            if (removedNews == null)
            {
                throw new NullReferenceException();
            }
            var slides = await _db.HomeMainCarouselItems.Where(s => s.NewsId == newsId).ToListAsync();
            if (slides.Count>0)
            {
                _db.HomeMainCarouselItems.RemoveRange(slides);
            }
            _db.News.Remove(removedNews);
            await _db.SaveChangesAsync();
        }


        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<NewsView> GetNews(int? newsId)
        {
            if (newsId == null)
            {
                throw new ArgumentNullException();
            }
            var news = await _db.News.FindAsync(newsId);
            if (news == null)
            {
                throw new NullReferenceException();
            }
            var newsView = new NewsView(news.Title, news.NewsText, news.DateOfCreation.ToShortDateString());
            return newsView;

        }

        public async Task<EditNews> StartEditNews(int? newsId)
        {
            if (newsId == null)
            {
                throw new ArgumentNullException();
            }
            var news = await _db.News.FindAsync(newsId);
            if (news == null)
            {
                throw new NullReferenceException();
            }
            var newsView = new EditNews(news.Id, news.Title, news.NewsText);
            return newsView;
        }
    }
}
