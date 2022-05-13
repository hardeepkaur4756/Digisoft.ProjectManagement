using Digisoft.ProjectManagement.Models;

namespace Digisoft.ProjectManagement.Configuration
{
    public class AutoMapperProfile : AutoMapper.Profile
    {
        #region Constructor
        public AutoMapperProfile()
        {
            CreateMap<AspNetUser, UserViewModel>()
                 .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
            CreateMap<ClientViewModel, Client>();
            CreateMap<Client, ClientViewModel>();
            CreateMap<Project, ProjectViewModel>()
                 .ForMember(dest => dest.CreatedByName, opt => opt.MapFrom(src => src.AspNetUser.UserName))
                 .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.Client.Name));
            CreateMap<ProjectViewModel, Project>();
            CreateMap<WorkingViewModel, Working>();
            CreateMap<Working, WorkingViewModel>();
            CreateMap<UserDetail, UserViewModel>()
                 .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name))
            .ForMember(dest => dest.CurrentStateName, opt => opt.MapFrom(src => src.State.Name))
            .ForMember(dest => dest.CurrentCountryName, opt => opt.MapFrom(src => src.Country.Name))
            .ForMember(dest => dest.PermanentStateName, opt => opt.MapFrom(src => src.State1.Name))
            .ForMember(dest => dest.PermanentCountryName, opt => opt.MapFrom(src => src.Country1.Name))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.AspNetUser.Email));
            CreateMap<UserViewModel, UserDetail>();

        }
        #endregion

        public static void Run()
        {
            AutoMapper.Mapper.Initialize(a => { a.AddProfile<AutoMapperProfile>(); });
        }
    }
}