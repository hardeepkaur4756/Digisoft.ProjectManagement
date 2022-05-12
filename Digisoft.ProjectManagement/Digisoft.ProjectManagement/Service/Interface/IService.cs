using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Digisoft.ProjectManagement.Service.Interface
{
    public interface IService<TEntity> where TEntity : class
    {
        void Delete(int id);
        IEnumerable<TEntity> GetAll();
        TEntity GetByID(int id);
       
    }
}