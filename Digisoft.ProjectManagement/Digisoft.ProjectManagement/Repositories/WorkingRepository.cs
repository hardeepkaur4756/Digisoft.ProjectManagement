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
            return _context.Workings.Include(x => x.AspNetUser);
        }
        public IEnumerable<Working> GetAll(DateTime? startDate, DateTime? endDate)
        {
            if (startDate.HasValue && endDate.HasValue)
            {
                return _context.Workings.Where(x =>
                (x.CreatedDate.Day >= ((DateTime)startDate).Day
                && x.CreatedDate.Month >= ((DateTime)startDate).Month
                && x.CreatedDate.Year >= ((DateTime)startDate).Year)
                &&
                (x.CreatedDate.Day <= ((DateTime)endDate).Day
                && x.CreatedDate.Month <= ((DateTime)endDate).Month
                && x.CreatedDate.Year <= ((DateTime)endDate).Year)
                );
            }
            else
            {
                return _context.Workings;
            }
        }

        public IEnumerable<Working> GetAllAfterSearch(DataTablesParam param, DateTime? startDate, DateTime? endDate)
        {
            var sSearch = param.sSearch.ToLower();
            if (startDate.HasValue && endDate.HasValue)
            {
                return _context.Workings
                      .Where(x =>
                      (x.CreatedDate >= startDate && x.CreatedDate <= endDate) &&
                        (x.Description.Contains(sSearch)
                      || x.AspNetUser.UserName.ToLower().Contains(sSearch))
                      || x.CreatedDate.ToString().Contains(sSearch)
                      );
            }
            else
            {
                return _context.Workings.Where(x =>
                         x.Description.Contains(sSearch)
                         || x.AspNetUser.UserName.ToLower().Contains(sSearch)
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