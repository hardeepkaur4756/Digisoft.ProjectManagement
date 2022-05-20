using Digisoft.ProjectManagement.Models;
using Digisoft.ProjectManagement.Repositories;
using Digisoft.ProjectManagement.Service.Interface;
using System;
using System.Collections.Generic;

namespace Digisoft.ProjectManagement.Service
{
    public class WorkingService : IWorkingService
    {
        private readonly WorkingRepository _workingRepository;

        public WorkingService()
        {
            _workingRepository = new WorkingRepository();
        }

        public void Delete(int id)
        {
            _workingRepository.Delete(id);
        }
        public void Delete(WorkingViewModel workingVM)
        {
            Working workingEntity = _workingRepository.GetbyId(workingVM.Id);
            workingVM.HoursBilled = workingEntity.HoursBilled;
            workingVM.HoursWorked = workingEntity.HoursWorked;
            workingVM.DateWorked = workingEntity.DateWorked;
            workingVM.CreatedBy = workingEntity.CreatedBy;
            workingVM.CreatedDate = workingEntity.CreatedDate;
            workingVM.ProjectId = workingEntity.ProjectId;
            AutoMapper.Mapper.Map(workingVM, workingEntity);
            _workingRepository.Update(workingEntity);
        }
        public IEnumerable<Working> GetAll()
        {
            return _workingRepository.GetAll();
        }
        public IEnumerable<Working> GetAll(DateTime? startDate, DateTime? endDate, int projectId,string userId)
        {
            return _workingRepository.GetAll(startDate, endDate,projectId,userId);
        }

        public IEnumerable<Working> GetAllAfterSearch(DataTablesParam param, DateTime? startDate, DateTime? endDate, int projectId,string userId)
        {
            return _workingRepository.GetAllAfterSearch(param, startDate, endDate, projectId,userId);
        }

        /// <summary>
        /// get Project by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Working GetByID(int id)
        {
            return _workingRepository.GetbyId(id);
        }

        /// <summary>
        /// insert or update Projects
        /// </summary>
        /// <param name="projectVM"></param>
        /// <returns></returns>
        public Working InsertUpdate(WorkingViewModel workingVM)
        {
            try
            {
                if (workingVM != null)
                {
                    // Get exitting project by Id
                    Working working = _workingRepository.GetbyId(workingVM.Id);

                    if (workingVM.Id > 0)
                    {
                        workingVM.CreatedDate = working.CreatedDate;
                        workingVM.CreatedBy = working.CreatedBy;
                        AutoMapper.Mapper.Map(workingVM, working);
                        _workingRepository.Update(working);
                        return working;
                    }
                    else
                    {  // insert project 
                        workingVM.CreatedDate = DateTime.Now;
                        Working workingEntity = new Working();
                        AutoMapper.Mapper.Map(workingVM, workingEntity);
                        _workingRepository.Insert(workingEntity);
                        return workingEntity;
                    }
                }
                else
                {
                    throw new Exception("workingVM is null.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public WorkingViewModel GetByDigisoftIDVM(int id)
        {
            Working working = _workingRepository.GetbyId(id);
            WorkingViewModel workingVM = new WorkingViewModel();
            AutoMapper.Mapper.Map(working, workingVM);
            return workingVM;
        }
        public WorkingViewModel GetByIDVM(int id)
        {
            Working working = _workingRepository.GetbyId(id);
            WorkingViewModel workingVM = new WorkingViewModel();
            AutoMapper.Mapper.Map(working, workingVM);
            return workingVM;
        }

        Working IWorkingService.InsertUpdate(WorkingViewModel projectVM)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Working> IService<Working>.GetAll()
        {
            throw new NotImplementedException();
        }

        Working IService<Working>.GetByID(int id)
        {
            throw new NotImplementedException();
        }
    }
}