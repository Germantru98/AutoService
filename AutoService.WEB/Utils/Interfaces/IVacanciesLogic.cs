using AutoService.WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.WEB.Utils.Interfaces
{
    public interface IVacanciesLogic:IDisposable
    {
        Task AddNewVacancy(JobVacancy newVacancy);
        Task RemoveVacancy(int? id);
        Task EditVacancy(JobVacancy editeVacancy);
        Task<List<JobVacancy>> GetAllVacancies();
        Task<JobVacancy> FindVacancy(int? vacancyId);
    }
}
