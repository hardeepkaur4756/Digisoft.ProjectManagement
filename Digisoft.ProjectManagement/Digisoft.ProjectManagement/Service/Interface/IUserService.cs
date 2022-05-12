using Digisoft.ProjectManagement.Models;

namespace Digisoft.ProjectManagement.Service.Interface
{
    public interface IUserService : IService<AspNetUser>
    {
        AspNetUser Insert(UserViewModel userVM);
    }
}
