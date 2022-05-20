using Digisoft.ProjectManagement.Models;
using Digisoft.ProjectManagement.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Digisoft.ProjectManagement.Repositories
{
    public class UserRepository : IRepository<AspNetUser>
    {
        private readonly ProjectManagementEntities _context;
        public UserRepository(ProjectManagementEntities context)
        {
            _context = context;
        }
        public UserRepository()
        {
            _context = new ProjectManagementEntities();
        }
        public IEnumerable<AspNetUser> GetAll()
        {
            return _context.AspNetUsers;
        }

        public IEnumerable<UserDetail> GetAllUser()
        {
            return _context.UserDetails;
        }
        public AspNetUser GetbyId(string id)
        {
            return _context.AspNetUsers.FirstOrDefault(x => x.Id == id);
        }
        public UserDetail GetbyIdUser(string id)
        {
            return _context.UserDetails.FirstOrDefault(x => x.UserId == id);
        }
        
        public void Delete(int id)
        {
            AspNetUser model = _context.AspNetUsers.FirstOrDefault(x => x.Id == id.ToString());
            _context.AspNetUsers.Remove(model);
            _context.SaveChanges();
        }
        public UserDetail Update(UserDetail user)
        {
            _context.UserDetails.Attach(user);
            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();
            return user;
        }
        public AspNetUser Insert(UserDetail user)
        {
            AspNetUser u = new AspNetUser();
            _context.UserDetails.Add(user);
            _context.SaveChanges();
            return u;
        }
        public AspNetUser Insert(AspNetUser entity)
        {
            throw new NotImplementedException();
        }

        public AspNetUser Update(AspNetUser entity)
        {
            throw new NotImplementedException();
        }

        public AspNetUser GetbyId(int id)
        {
            throw new NotImplementedException();
        }
    }
}