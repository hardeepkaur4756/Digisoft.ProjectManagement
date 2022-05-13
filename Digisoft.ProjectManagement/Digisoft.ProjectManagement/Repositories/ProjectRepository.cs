using Digisoft.ProjectManagement.Models;
using Digisoft.ProjectManagement.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Digisoft.ProjectManagement.Repositories
{
    public class ProjectRepository : IRepository<Project>
    {
        private readonly ProjectManagementEntities _context;
        public ProjectRepository(ProjectManagementEntities context)
        {
            _context = context;
        }
        public ProjectRepository()
        {
            _context = new ProjectManagementEntities();
        }
        public IEnumerable<Project> GetAll()
        {
            return _context.Projects.Include(x => x.AspNetUser).Where(x=>x.IsActive==true);
        }
        public IEnumerable<Project> GetAllForFilter()
        {
            return _context.Projects.Include(x => x.AspNetUser);
        }
        public IEnumerable<Project> GetAll(DateTime? startDate, DateTime? endDate)
        {
            if (startDate.HasValue && endDate.HasValue)
            {
                return _context.Projects.Where(x =>
                (x.CreatedOn.Day >= ((DateTime)startDate).Day
                && x.CreatedOn.Month >= ((DateTime)startDate).Month
                && x.CreatedOn.Year >= ((DateTime)startDate).Year)
                &&
                (x.CreatedOn.Day <= ((DateTime)endDate).Day
                && x.CreatedOn.Month <= ((DateTime)endDate).Month
                && x.CreatedOn.Year <= ((DateTime)endDate).Year)
                && x.IsActive == true
                );
            }
            else
            {
                return _context.Projects.Where(x => x.IsActive == true);
            }
        }

        public IEnumerable<Project> GetAllAfterSearch(DataTablesParam param, DateTime? startDate, DateTime? endDate)
        {
            var sSearch = param.sSearch.ToLower();
            if (startDate.HasValue && endDate.HasValue)
            {
                return _context.Projects
                      .Where(x =>
                      (x.CreatedOn >= startDate && x.CreatedOn <= endDate) &&
                        (x.Name.ToLower().Contains(sSearch)
                      || x.AspNetUser.UserName.ToLower().Contains(sSearch))
                      || x.CreatedOn.ToString().Contains(sSearch) && x.IsActive == true
                      );
            }
            else
            {
                return _context.Projects.Where(x =>
                         x.Name.ToLower().Contains(sSearch)
                         || x.AspNetUser.UserName.ToLower().Contains(sSearch) && x.IsActive == true
                         );
            }
        }

        public Project GetbyId(int id)
        {
            return _context.Projects.FirstOrDefault(x => x.Id == id);
        }
        public void Delete(int id)
        {
            Project model = _context.Projects.FirstOrDefault(x => x.Id == id);
            _context.Projects.Remove(model);
            _context.SaveChanges();
        }

        public Project Update(Project project)
        {
            _context.SaveChanges();
            return project;
        }

        public Project Insert(Project project)
        {
            _context.Projects.Add(project);
            _context.SaveChanges();
            return project;
        }
    }
}