using Digisoft.ProjectManagement.Models;
using Digisoft.ProjectManagement.Repositories;
using Digisoft.ProjectManagement.Service.Interface;
using System;
using System.Collections.Generic;

namespace Digisoft.ProjectManagement.Service
{
    public class ProjectService : IProjectService
    {
        private readonly ProjectRepository _projectRepository;

        public ProjectService()
        {
            _projectRepository = new ProjectRepository();
        }

        public void Delete(int id)
        {
            _projectRepository.Delete(id);
        }

        public IEnumerable<Project> GetAll()
        {
            return _projectRepository.GetAll();
        }
        public IEnumerable<Project> GetAll(DateTime? startDate, DateTime? endDate)
        {
            return _projectRepository.GetAll(startDate, endDate);
        }

        public IEnumerable<Project> GetAllAfterSearch(DataTablesParam param, DateTime? startDate, DateTime? endDate)
        {
            return _projectRepository.GetAllAfterSearch(param, startDate, endDate);
        }

        /// <summary>
        /// get Project by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Project GetByID(int id)
        {
            return _projectRepository.GetbyId(id);
        }

        /// <summary>
        /// insert or update Projects
        /// </summary>
        /// <param name="projectVM"></param>
        /// <returns></returns>
        public Project InsertUpdate(ProjectViewModel projectVM)
        {
            try
            {
                if (projectVM != null)
                {
                    // Get exitting project by Id
                    Project project = _projectRepository.GetbyId(projectVM.Id);

                    if (projectVM.Id > 0)
                    {
                        projectVM.CreatedOn = project.CreatedOn;
                        projectVM.CreatedBy = project.CreatedBy;
                        AutoMapper.Mapper.Map(projectVM, project);
                        _projectRepository.Update(project);
                        return project;
                    }
                    else
                    {  // insert project 
                        projectVM.CreatedOn = DateTime.Now;
                        Project projectEntity = new Project();
                        AutoMapper.Mapper.Map(projectVM, projectEntity);
                        _projectRepository.Insert(projectEntity);
                        return projectEntity;
                    }
                }
                else
                {
                    throw new Exception("projectVM is null.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public ProjectViewModel GetByDigisoftIDVM(int id)
        {
            Project project = _projectRepository.GetbyId(id);
            ProjectViewModel projectVM = new ProjectViewModel();
            AutoMapper.Mapper.Map(project, projectVM);
            return projectVM;
        }
        public ProjectViewModel GetByIDVM(int id)
        {
            Project project = _projectRepository.GetbyId(id);
            ProjectViewModel projectVM = new ProjectViewModel();
            AutoMapper.Mapper.Map(project, projectVM);
            return projectVM;
        }

        Project IProjectService.InsertUpdate(ProjectViewModel projectVM)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Project> IService<Project>.GetAll()
        {
            throw new NotImplementedException();
        }

        Project IService<Project>.GetByID(int id)
        {
            throw new NotImplementedException();
        }
    }
}