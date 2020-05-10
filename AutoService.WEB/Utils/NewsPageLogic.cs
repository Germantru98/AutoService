using AutoService.WEB.Models;
using AutoService.WEB.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace AutoService.WEB.Utils
{
    public class NewsPageLogic : INewsPageLogic
    {
        private ApplicationDbContext _db;

        public NewsPageLogic()
        {
        }

        public NewsPageLogic(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task AddNews(CreateNews news)
        {
            var item = new News()
            {
                Title = news.Title,
                NewsText = news.NewsText,
                SlideDescription = news.SlideDescription,
                SlideTitle = news.SlideTitle,
                ImgHref = news.ImgHref,
                DateOfCreation = DateTime.Today,
            };
            _db.NewsContext.Add(item);
            await _db.SaveChangesAsync();
        }

        public async Task EditNews(EditNews editedNews)
        {
            var news = await _db.NewsContext.FindAsync(editedNews.Id);
            news.Title = editedNews.Title;
            news.NewsText = editedNews.NewsText;
            news.SlideTitle = editedNews.SlideTitle;
            news.SlideDescription = editedNews.SlideDescription;
            news.ImgHref = editedNews.ImgHref;
            news.DateOfCreation = DateTime.Today;
            _db.Entry(news).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task<List<News>> GetAllNews()
        {
            var result = await _db.NewsContext.ToListAsync();
            return result;
        }

        public async Task RemoveNews(int? newsId)
        {
            if (newsId == null)
            {
                throw new ArgumentNullException();
            }
            var removedNews = await _db.NewsContext.FindAsync(newsId);
            if (removedNews == null)
            {
                throw new NullReferenceException();
            }
            _db.NewsContext.Remove(removedNews);
            await _db.SaveChangesAsync();
        }

        public async Task<NewsView> GetNews(int? newsId)
        {
            if (newsId == null)
            {
                throw new ArgumentNullException();
            }
            var news = await _db.NewsContext.FindAsync(newsId);
            if (news == null)
            {
                throw new NullReferenceException();
            }
            var newsView = new NewsView(news.Title, news.NewsText, news.DateOfCreation.ToShortDateString(), news.ImgHref);
            return newsView;
        }

        public async Task<EditNews> StartEditNews(int? newsId)
        {
            if (newsId == null)
            {
                throw new ArgumentNullException();
            }
            var news = await _db.NewsContext.FindAsync(newsId);
            if (news == null)
            {
                throw new NullReferenceException();
            }
            var newsView = new EditNews()
            {
                Id = news.Id,
                Title = news.Title,
                NewsText = news.NewsText,
                SlideTitle = news.SlideTitle,
                SlideDescription = news.SlideDescription,
                ImgHref = news.ImgHref
            };
            return newsView;
        }

        public async Task<List<Slide>> GetNewsSlides()
        {
            var news = await _db.NewsContext.ToListAsync();
            var result = new List<Slide>();
            foreach (var item in news)
            {
                var slide = new Slide(item.Id, item.SlideTitle, item.SlideDescription, item.ImgHref);
                result.Add(slide);
            }
            return result;
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
    }
}