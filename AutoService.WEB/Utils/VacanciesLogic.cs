using AutoService.WEB.Models;
using AutoService.WEB.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace AutoService.WEB.Utils
{
    public class VacanciesLogic : IVacanciesLogic
    {
        private ApplicationDbContext _db;

        public VacanciesLogic()
        {
        }

        public VacanciesLogic(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task AddNewVacancy(JobVacancy newVacancy)
        {
            _db.JobVacancies.Add(newVacancy);
            await _db.SaveChangesAsync();
        }

        public async Task EditVacancy(JobVacancy editeVacancy)
        {
            var vacancy = await _db.JobVacancies.FindAsync(editeVacancy.Id);
            vacancy.Salary = editeVacancy.Salary;
            vacancy.VacancyDescription = editeVacancy.VacancyDescription;
            vacancy.VacancyName = editeVacancy.VacancyName;
            vacancy.ContactPhone = editeVacancy.ContactPhone;
            vacancy.Email = editeVacancy.Email;
            _db.Entry(vacancy).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task<JobVacancy> FindVacancy(int? vacancyId)
        {
            if (vacancyId == null)
            {
                throw new ArgumentNullException();
            }
            var vacancy = await _db.JobVacancies.FindAsync(vacancyId);
            if (vacancy == null)
            {
                throw new NullReferenceException();
            }
            return vacancy;
        }

        public async Task<List<JobVacancy>> GetAllVacancies()
        {
            return await _db.JobVacancies.ToListAsync();
        }

        public async Task RemoveVacancy(int? vacancyId)
        {
            if (vacancyId == null)
            {
                throw new ArgumentNullException();
            }
            var vacancy = await _db.JobVacancies.FindAsync(vacancyId);
            if (vacancy == null)
            {
                throw new NullReferenceException();
            }
            _db.JobVacancies.Remove(vacancy);
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
    }
}