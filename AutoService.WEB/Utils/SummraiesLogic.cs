using AutoService.WEB.Models;
using AutoService.WEB.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AutoService.WEB.Utils
{
    public class SummraiesLogic : ISummariesLogic
    {
        private ApplicationDbContext _db;
        private ICarLogic _carLogic;
        private IServicesLogic _servicesLogic;

        public SummraiesLogic(ApplicationDbContext db, ICarLogic carLogic, IServicesLogic servicesLogic)
        {
            _db = db;
            _carLogic = carLogic;
            _servicesLogic = servicesLogic;
        }

        public async Task CompleteSummary(int summaryId)
        {
            var isCompletedTest = await _db.CompletedSummariesHistory.Where(s => s.SummaryId == summaryId).ToListAsync();
            if (isCompletedTest.Count > 0)
            {
                throw new ArgumentException($"Заказ с summaryId = {summaryId} уже выполнен");
            }
            var summaryFromDb = await _db.ServicesSummaries.FindAsync(summaryId);
            summaryFromDb.IsCompleted = true;
            var completedSummaryHistory = new CompletedSummariesHistory()
            {
                SummaryId = summaryFromDb.SummaryId,
                DateOfCompletion = DateTime.Now
            };
            _db.CompletedSummariesHistory.Add(completedSummaryHistory);
            _db.Entry(summaryFromDb).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task EditSummary(EditSummaryView editedSummary)
        {
            var summaryFromDb = await _db.ServicesSummaries.FindAsync(editedSummary.SummaryId);
            var newServiceList = string.Join("|", editedSummary.SelectedServices);
            summaryFromDb.UserCarId = editedSummary.UserCarId;
            summaryFromDb.DayOfWork = editedSummary.DayOfWork;
            summaryFromDb.ServiceList = newServiceList;
            summaryFromDb.TotalPrice = await GetNewPrice(editedSummary.SelectedServices);
            _db.Entry(summaryFromDb).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        private async Task<int> GetNewPrice(IList<string> servicesId)
        {
            int result = 0;
            foreach (var serviceId in servicesId)
            {
                int Id = int.Parse(serviceId);
                var tmpService = await _servicesLogic.FindServiceWithDiscount(Id);
                if (tmpService.Discount != null)
                {
                    result += tmpService.PriceWithDiscount;
                }
                else
                {
                    result += tmpService.Price;
                }
            }
            return result;
        }

        private string ServicesListToString(List<Service> services)
        {
            var result = string.Empty;
            for (int i = 0; i < services.Count; i++)
            {
                if (i < services.Count - 1)
                {
                    result += $"{services[i].ServiceId}|";
                }
                else
                {
                    result += $"{services[i].ServiceId}";
                }
            }
            return result;
        }

        public async Task<ServicesSummaryAdminView> FindSummaryById(int? summaryId)
        {
            if (summaryId == null)
            {
                throw new ArgumentNullException($"Параметр summaryId = {summaryId}");
            }
            else
            {
                var summary = await _db.ServicesSummaries.FindAsync(summaryId);
                if (summary == null)
                {
                    throw new NullReferenceException($"Объект с summaryId = {summaryId} отсутствует в базе данных");
                }
                else
                {
                    return await MapSummaryOnSummaryAdminView(summary);
                }
            }
        }

        public async Task<List<ServicesSummaryAdminView>> GetCompletedSummaries()
        {
            var completedSummariesFromDb = await _db.CompletedSummariesHistory.ToListAsync();
            var result = new List<ServicesSummaryAdminView>();
            foreach (var completedSummary in completedSummariesFromDb)
            {
                var tmpSummary = await MapSummaryOnSummaryAdminView(await _db.ServicesSummaries.FindAsync(completedSummary.SummaryId));
                result.Add(tmpSummary);
            }
            return result;
        }

        public async Task<List<ServicesSummaryAdminView>> GetCurrentSummaries()
        {
            var curDate = DateTime.Today;
            var resultFromDb = await _db.ServicesSummaries.Where(s => s.DayOfWork == curDate && !s.IsCompleted).ToListAsync();
            var result = await MapDbResultToList(resultFromDb);
            return result;
        }

        private async Task<List<ServicesSummaryAdminView>> MapDbResultToList(List<ServicesSummary> resultFromDb)
        {
            var result = new List<ServicesSummaryAdminView>();
            foreach (var summary in resultFromDb)
            {
                var tmpSummary = await MapSummaryOnSummaryAdminView(summary);
                result.Add(tmpSummary);
            }
            return result;
        }

        public async Task RemoveSummary(int summaryId)
        {
            var deletedSummary = await _db.ServicesSummaries.FindAsync(summaryId);
            _db.ServicesSummaries.Remove(deletedSummary);
            await _db.SaveChangesAsync();
        }

        private async Task<ServicesSummaryAdminView> MapSummaryOnSummaryAdminView(ServicesSummary summary)
        {
            var tmpUser = _db.Users.Find(summary.UserId);
            var tmpCarFromSummary = _carLogic.MapCarToCarView(await _db.Cars.FindAsync(summary.UserCarId));
            var servicesList = await _servicesLogic.GetServicesFromSummary(summary.ServiceList);
            return new ServicesSummaryAdminView(summary.SummaryId, tmpUser, servicesList, summary.TotalPrice, tmpCarFromSummary, summary.DayOfWork, summary.IsCompleted);
        }

        public async Task<List<ServicesSummaryAdminView>> GetAllUncompletedSummaries()
        {
            var resultFromDb = await _db.ServicesSummaries.Where(s => !s.IsCompleted).ToListAsync();
            var result = await MapDbResultToList(resultFromDb);
            return result;
        }

        public async Task<List<ServicesSummaryAdminView>> GetCompletedSummariesByPeriod(DateTime start, DateTime finish)
        {
            var resultFromDb = await _db.ServicesSummaries.Where(s => s.DayOfWork >= start && s.DayOfWork <= finish && s.IsCompleted).ToListAsync();
            var result = await MapDbResultToList(resultFromDb);
            return result;
        }

        public async Task<List<ServicesSummaryAdminView>> GetCompletedSummariesByDate(DateTime date)
        {
            var resultFromDb = await _db.ServicesSummaries.Where(s => s.DayOfWork == date && s.IsCompleted).ToListAsync();
            var result = await MapDbResultToList(resultFromDb);
            return result;
        }

        public async Task<EditSummaryView> GetEditSummaryView(ServicesSummaryAdminView view)
        {
            var selectListItems = await GetAllServices();
            var servisesId = GetServicesId(view.ServicesList);
            if (selectListItems.Count > 0)
            {
                var editSummaryView = new EditSummaryView(view.SummaryId, view.Car.CarId, view.Date, servisesId, selectListItems);
                return editSummaryView;
            }
            else
            {
                throw new Exception("List is empty");
            }
        }

        private string[] GetServicesId(List<Service> services)
        {
            string[] result = new string[services.Count];
            for (int i = 0; i < services.Count; i++)
            {
                result[i] = $"{services[i].ServiceId}";
            }
            return result;
        }

        private async Task<List<SelectListItem>> GetAllServices()
        {
            var result = new List<SelectListItem>();
            var resultFromDb = await _db.Services.ToListAsync();
            foreach (var service in resultFromDb)
            {
                result.Add(new SelectListItem() { Text = service.ServiceName, Value = $"{service.ServiceId}" });
            }
            return result;
        }
    }
}