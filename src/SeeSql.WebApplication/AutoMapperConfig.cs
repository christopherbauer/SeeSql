using AutoMapper;
using BpCommon.Models.SqlVisibility;
using SeeSql.ApplicationServices;
using SeeSql.DomainModels;

namespace SeeSql.WebApplication
{
    public class AutoMapperConfig
    {
        public static Mapper RegisterMappings()
        {
            var mapper = new Mapper(new MapperConfiguration(expression =>
            {
                expression.CreateMap<sysjob, Job>();
                expression.CreateMap<sysjobstep, JobStep>();
            }));
            return mapper;
        }
    }
}