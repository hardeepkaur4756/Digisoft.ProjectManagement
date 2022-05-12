using Digisoft.ProjectManagement.Models;
using Digisoft.ProjectManagement.Repositories;
using Digisoft.ProjectManagement.Service.Interface;
using System;
using System.Collections.Generic;

namespace Digisoft.ProjectManagement.Service
{
    public class UserService : IUserService
    {
        private readonly UserRepository _userRepository;

        public UserService()
        {
            _userRepository = new UserRepository();
        }
        public void Delete(int id)
        {
            _userRepository.Delete(id);
        }

        public IEnumerable<AspNetUser> GetAll()
        {
            return _userRepository.GetAll();
        }
        public IEnumerable<UserDetail> GetAllUser()
        {
            return _userRepository.GetAllUser();
        }

        public AspNetUser GetByID(int id)
        {
            return _userRepository.GetbyId(id.ToString());
        }
        public UserViewModel GetByIDVM(string id)
        {
            UserDetail user = _userRepository.GetbyIdUser(id);
            UserViewModel userVM = new UserViewModel();
            userVM.UserId = user.UserId;
            userVM.DepartmentId = user.DepartmentId ?? 0;
            AutoMapper.Mapper.Map(user, userVM);
            return userVM;
        }
        /// <summary>
        /// insert or update user
        /// </summary>
        /// <param name="userVM"></param>
        /// <returns></returns>
        public UserDetail Insert(UserViewModel userVM)
        {
            try
            {
                if (userVM != null)
                {
                    UserDetail userEntity = new UserDetail();
                    userVM.Exclude = userEntity.Exclude ?? false;
                    AutoMapper.Mapper.Map(userVM, userEntity);
                    _userRepository.Insert(userEntity);
                    return userEntity;

                }
                else
                {
                    throw new Exception("userVM is null.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        AspNetUser IUserService.Insert(UserViewModel userVM)
        {
            throw new NotImplementedException();
        }
        public UserDetail UpdateStatus(UserViewModel model)
        {
            try
            {
                if (model != null)
                {
                    UserDetail userEntity = new UserDetail();
                    userEntity.Exclude = model.Exclude;
                    userEntity.UserId = model.UserId;
                    _userRepository.Update(userEntity);
                    return userEntity;
                }
                else
                {
                    throw new Exception("model is null.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}