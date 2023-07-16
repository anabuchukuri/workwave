using AutoMapper;
using WorkWave.DBModels;
using WorkWave.Dtos.JobOpeningDtos;

namespace WorkWave.Dtos
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            /*JobOpening*/
            CreateMap<JobOpening, JobOpeningDto>();
            CreateMap<JobOpeningDto, JobOpening>();

            CreateMap<JobOpening, JobOpeningAddDto>();
            CreateMap<JobOpeningAddDto, JobOpening>();

            /*JobType*/
            CreateMap<JobType, JobTypeDto>();
            CreateMap<JobTypeDto, JobOpening>();

            CreateMap<JobType, JobTypeAddDto>();
            CreateMap<JobTypeAddDto, JobType>();

            /*JobCategory*/
            CreateMap<JobCategory, JobCategoryDto>();
            CreateMap<JobCategoryDto, JobCategory>();

            CreateMap<JobCategory, JobCategoryAddDto>();
            CreateMap<JobCategoryAddDto, JobCategory>();
        }
    }
}
