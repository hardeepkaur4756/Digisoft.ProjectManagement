using Digisoft.ProjectManagement.Models;

namespace Digisoft.ProjectManagement.Service.Interface
{
    public interface IUserAttendanceService : IService<UserAttendance>
    {
        UserAttendanceViewModel GetByIDVM(int id);
        UserAttendance InsertUpdate(UserAttendanceViewModel attendanceVM);
    }
}
