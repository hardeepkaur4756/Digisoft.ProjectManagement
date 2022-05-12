using Digisoft.ProjectManagement.Models;

namespace Digisoft.ProjectManagement.Service.Interface
{
    public interface IProjectService : IService<Project>
    {
        ProjectViewModel GetByIDVM(int id);
        Project InsertUpdate(ProjectViewModel clientVM);
    }
}
