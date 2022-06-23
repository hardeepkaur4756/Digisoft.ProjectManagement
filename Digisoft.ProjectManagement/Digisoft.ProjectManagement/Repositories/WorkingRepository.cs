using Digisoft.ProjectManagement.Models;
using Digisoft.ProjectManagement.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Digisoft.ProjectManagement.Repositories
{
    public class WorkingRepository : IRepository<Working>
    {
        private readonly ProjectManagementEntities _context;
        public WorkingRepository(ProjectManagementEntities context)
        {
            _context = context;
        }
        public WorkingRepository()
        {
            _context = new ProjectManagementEntities();
        }
        public IEnumerable<Working> GetAll()
        {
            return _context.Workings.Include(x => x.AspNetUser).Where(x => x.IsActive == true);
        }
        public IEnumerable<Working> GetAll(DateTime? startDate, DateTime? endDate, int projectId, string userId)
        {
            var workings = _context.Workings.Where(x => x.IsActive == true).AsQueryable();
            if (startDate.HasValue && endDate.HasValue)
            {
                workings = workings.Where(x => x.DateWorked >= (DateTime)startDate
                && x.DateWorked <= (DateTime)endDate);
            }
            if (projectId != 0)
            {
                workings = workings.Where(x => x.ProjectId == projectId);
            }
            if (!string.IsNullOrEmpty(userId))
            {
                workings = workings.Where(x => x.CreatedBy == userId);
            }
            return workings.AsEnumerable();
        }

        public IEnumerable<Working> GetAllAfterSearch(DataTablesParam param, DateTime? startDate, DateTime? endDate, int projectId, string userId)
        {
            var sSearch = param.sSearch.ToLower();
            if (startDate.HasValue && endDate.HasValue)
            {
                return _context.Workings
                      .Where(x => x.IsActive == true &&
                      (x.DateWorked >= startDate && x.DateWorked <= endDate) &&
                        (x.Description.Contains(sSearch)
                      || x.Project.Name.ToLower().Contains(sSearch)
                      || x.HoursWorked.ToString().ToLower().Contains(sSearch)
                      || x.HoursBilled.ToString().ToLower().Contains(sSearch)
                      || x.AspNetUser.UserName.ToLower().Contains(sSearch))
                      );
            }
            else
            {
                return _context.Workings.Where(x =>
                         x.Description.Contains(sSearch)
                         || x.Project.Name.ToLower().Contains(sSearch)
                         || x.HoursWorked.ToString().ToLower().Contains(sSearch)
                         || x.HoursBilled.ToString().ToLower().Contains(sSearch)
                         || x.AspNetUser.UserName.ToLower().Contains(sSearch) && x.IsActive == true
                         || x.ProjectId == projectId
                         || x.CreatedBy == (userId ?? x.CreatedBy)
                         );
            }
        }

        public Working GetbyId(int id)
        {
            return _context.Workings.FirstOrDefault(x => x.Id == id);
        }
        public void Delete(int id)
        {
            Working model = _context.Workings.FirstOrDefault(x => x.Id == id);
            _context.Workings.Remove(model);
            _context.SaveChanges();
        }

        public Working Update(Working Working)
        {
            _context.SaveChanges();
            return Working;
        }

        public Working Insert(Working Working)
        {
            _context.Workings.Add(Working);
            _context.SaveChanges();
            return Working;
        }
    }
}