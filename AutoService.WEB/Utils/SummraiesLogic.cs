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
            var today = DateTime.Today;
            var isOrderAlreadyCompletedTest = await _db.CompletedSummariesHistory.FirstOrDefaultAsync(s => s.SummaryId == summaryId);
            if (isOrderAlreadyCompletedTest != null)
            {
                throw new ArgumentException($"Заказ с summaryId = {summaryId} уже выполнен");
            }
            var summaryFromDb = await _db.ServicesSummaries.FindAsync(summaryId);
            if (summaryFromDb.DayOfWork != today)
            {
                throw new Exception("Ошибка,невозможно завершить заказ, так как дата работ не совпадает с текущей датой");
            }
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

        public async Task<ServicesSummaryAdminView> FindSummaryById(int? summaryId)
        {
            if (summaryId == null)
            {
                throw new ArgumentNullException($"Параметр summaryId = null");
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
            return result.OrderByDescending(s => s.Date).ToList();
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

        public async Task RemoveSummary(int summaryId, string userId, bool isAdmin)
        {
            var deletedSummary = await _db.ServicesSummaries.FindAsync(summaryId);
            if (deletedSummary.UserId != userId && !isAdmin)
            {
                throw new InvalidOperationException();
            }
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
            var resultFromDb = await _db.ServicesSummaries.Where(s => !s.IsCompleted).OrderBy(s => s.DayOfWork).ToListAsync();
            resultFromDb = await RemoveNotActualOrders(resultFromDb);
            var result = await MapDbResultToList(resultFromDb);
            return result;
        }

        private async Task<List<ServicesSummary>> RemoveNotActualOrders(List<ServicesSummary> summaries)
        {
            var today = DateTime.Today;
            foreach (var order in summaries)
            {
                if (order.DayOfWork < today)
                {
                    _db.ServicesSummaries.Remove(order);
                }
            }
            await _db.SaveChangesAsync();
            summaries.RemoveAll(s => s.DayOfWork < today);
            return summaries;
        }

        public async Task<List<ServicesSummaryAdminView>> GetCompletedSummariesByPeriod(DateTime start, DateTime finish)
        {
            var resultFromDb = await _db.ServicesSummaries.Where(s => s.DayOfWork >= start && s.DayOfWork <= finish && s.IsCompleted).OrderBy(s => s.DayOfWork).ToListAsync();
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

        public async Task<EditSummaryView> GetEditSummaryView(UserOrderView view)
        {
            var selectListItems = await GetAllServices();
            var servisesId = GetServicesId(view.ServicesList);
            if (selectListItems.Count > 0)
            {
                var editSummaryView = new EditSummaryView(view.OrderId, view.UserCar.CarId, DateTime.Parse(view.selectedDateTime), servisesId, selectListItems);
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

        public ServicesSummaryView GetServicesSummaryView(IEnumerable<Service> shopCart, int totalPrice)
        {
            var view = new ServicesSummaryView()
            {
                ServicesList = shopCart,
                TotalPrice = totalPrice
            };
            return view;
        }

        public async Task AddNewSummary(ServicesSummary newSummary)
        {
            _db.ServicesSummaries.Add(newSummary);
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
                    _servicesLogic.Dispose();
                    _carLogic.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<List<UserOrderView>> GetAllUserOrders(string userId)
        {
            var userOrders = await _db.ServicesSummaries.Where(s => s.UserId == userId && !s.IsCompleted).OrderByDescending(s => s.DayOfWork).ToListAsync();
            var result = new List<UserOrderView>();
            foreach (var item in userOrders)
            {
                var serviceList = await _servicesLogic.GetServicesFromSummary(item.ServiceList);
                var userCar = await _carLogic.GetUserCar(item.UserCarId);
                result.Add(new UserOrderView(item.SummaryId, serviceList, item.TotalPrice, userCar, item.DayOfWork));
            }
            return result;
        }

        public async Task<UserOrderView> GetUserOrder(int? orderId, string userId)
        {
            if (orderId == null)
            {
                throw new ArgumentNullException();
            }
            var order = await _db.ServicesSummaries.FindAsync(orderId);
            if (order == null)
            {
                throw new NullReferenceException();
            }
            if (order.UserId != userId)
            {
                throw new InvalidOperationException();
            }
            var serviceList = await _servicesLogic.GetServicesFromSummary(order.ServiceList);
            var userCar = await _carLogic.GetUserCar(order.UserCarId);
            var userOrder = new UserOrderView(order.SummaryId, serviceList, order.TotalPrice, userCar, order.DayOfWork);
            return userOrder;
        }
    }
}