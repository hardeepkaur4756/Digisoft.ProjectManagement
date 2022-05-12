using Digisoft.ProjectManagement.Models;

namespace Digisoft.ProjectManagement.Service.Interface
{
    public interface IWorkingService : IService<Working>
    {
        WorkingViewModel GetByIDVM(int id);
        Working InsertUpdate(WorkingViewModel workingVM);
    }
}
