using Digisoft.ProjectManagement.Models;
using Digisoft.ProjectManagement.Repositories;
using Digisoft.ProjectManagement.Service.Interface;
using System;
using System.Collections.Generic;

namespace Digisoft.ProjectManagement.Service
{
    public class UserAttendanceService : IUserAttendanceService
    {
        private readonly UserAttendanceRepository _userAttendanceRepository;

        public UserAttendanceService()
        {
            _userAttendanceRepository = new UserAttendanceRepository();
        }
        public void Delete(int id)
        {
            _userAttendanceRepository.Delete(id);
        }
        public void Delete(UserAttendanceViewModel attendVm)
        {
            UserAttendance entity = _userAttendanceRepository.GetbyId(attendVm.Id);
            attendVm.CreatedDate = entity.CreatedDate;
            attendVm.CreatedBy = entity.CreatedBy;
            AutoMapper.Mapper.Map(attendVm, entity);
            _userAttendanceRepository.Update(entity);
        }
        public IEnumerable<UserAttendance> GetAll()
        {
            return _userAttendanceRepository.GetAll();
        }
        public IEnumerable<UserAttendance> GetAllForFilter()
        {
            return _userAttendanceRepository.GetAllForFilter();
        }
        public IEnumerable<UserAttendance> GetAll(DateTime? date)
        {
            return _userAttendanceRepository.GetAll(date);
        }
        public IEnumerable<UserAttendance> GetAllAfterSearch(DataTablesParam param, DateTime? date, string userId)
        {
            return _userAttendanceRepository.GetAllAfterSearch(param, date, userId);
        }
        /// <summary>
        /// get Project by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserAttendance GetByID(int id)
        {
            return _userAttendanceRepository.GetbyId(id);
        }

        /// <summary>
        /// insert or update Projects
        /// </summary>
        /// <param name="attendVM"></param>
        /// <returns></returns>
        public UserAttendance InsertUpdate(UserAttendanceViewModel attendVM)
        {
            try
            {
                if (attendVM != null)
                {
                    // Get exitting project by Id
                    UserAttendance attendance = _userAttendanceRepository.GetbyId(attendVM.Id);

                    if (attendVM.Id > 0)
                    {
                        attendVM.CreatedDate = attendance.CreatedDate;
                        attendVM.CreatedBy = attendance.CreatedBy;
                        AutoMapper.Mapper.Map(attendVM, attendance);
                        _userAttendanceRepository.Update(attendance);
                        return attendance;
                    }
                    else
                    {  // insert project 
                        attendVM.CreatedDate = DateTime.Now;
                        UserAttendance attendanceEntity = new UserAttendance();
                        AutoMapper.Mapper.Map(attendVM, attendanceEntity);
                        _userAttendanceRepository.Insert(attendanceEntity);
                        return attendanceEntity;
                    }
                }
                else
                {
                    throw new Exception("attendVM is null.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public UserAttendanceViewModel GetByDigisoftIDVM(int id)
        {
            UserAttendance attendance = _userAttendanceRepository.GetbyId(id);
            UserAttendanceViewModel attendanceVM = new UserAttendanceViewModel();
            AutoMapper.Mapper.Map(attendance, attendanceVM);
            return attendanceVM;
        }
        public UserAttendanceViewModel GetByIDVM(int id)
        {
            UserAttendance attendance = _userAttendanceRepository.GetbyId(id);
            UserAttendanceViewModel attendanceVM = new UserAttendanceViewModel();
            AutoMapper.Mapper.Map(attendance, attendanceVM);
            return attendanceVM;
        }
        UserAttendance IUserAttendanceService.InsertUpdate(UserAttendanceViewModel projectVM)
        {
            throw new NotImplementedException();
        }
        IEnumerable<UserAttendance> IService<UserAttendance>.GetAll()
        {
            throw new NotImplementedException();
        }
        UserAttendance IService<UserAttendance>.GetByID(int id)
        {
            throw new NotImplementedException();
        }
    }
}