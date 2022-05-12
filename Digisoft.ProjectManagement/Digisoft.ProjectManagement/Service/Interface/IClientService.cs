using Digisoft.ProjectManagement.Models;

namespace Digisoft.ProjectManagement.Service.Interface
{
    public interface IClientService : IService<Client>
    {
        ClientViewModel GetByIDVM(int id);
        Client InsertUpdate(ClientViewModel clientVM);
    }
}