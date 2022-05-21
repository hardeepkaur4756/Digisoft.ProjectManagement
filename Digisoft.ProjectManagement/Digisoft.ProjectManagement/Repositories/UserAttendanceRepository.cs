using Digisoft.ProjectManagement.Models;
using Digisoft.ProjectManagement.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Digisoft.ProjectManagement.Repositories
{
    public class UserAttendanceRepository : IRepository<UserAttendance>
    {
        private readonly ProjectManagementEntities _context;
        public UserAttendanceRepository(ProjectManagementEntities context)
        {
            _context = context;
        }
        public UserAttendanceRepository()
        {
            _context = new ProjectManagementEntities();
        }
        public IEnumerable<UserAttendance> GetAll()
        {
            return _context.UserAttendances.Include(x => x.AspNetUser);
        }
        public IEnumerable<UserAttendance> GetAllForFilter()
        {
            return _context.UserAttendances.Include(x => x.AspNetUser);
        }
        public IEnumerable<UserAttendance> GetAll(DateTime? date)
        {
            return _context.UserAttendances.Where(x => x.Date == date);
        }
        public IEnumerable<UserAttendance> GetAllAfterSearch(DataTablesParam param, DateTime? date, string userId)
        {
            var sSearch = param.sSearch.ToLower();

            return _context.UserAttendances.Where(x =>
                     x.AspNetUser.UserName.ToLower().Contains(sSearch) || x.UserId == userId || x.Date == date
                     );
        }
        public UserAttendance GetbyId(int id)
        {
            return _context.UserAttendances.FirstOrDefault(x => x.Id == id);
        }
        public void Delete(int id)
        {
            UserAttendance model = _context.UserAttendances.FirstOrDefault(x => x.Id == id);
            _context.UserAttendances.Remove(model);
            _context.SaveChanges();
        }

        public UserAttendance Update(UserAttendance attendance)
        {
            _context.SaveChanges();
            return attendance;
        }

        public UserAttendance Insert(UserAttendance attendance)
        {
            _context.UserAttendances.Add(attendance);
            _context.SaveChanges();
            return attendance;
        }
    }
}