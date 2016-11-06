using AutoMapper;
using SeeSql.ApplicationServices;
using SeeSql.DomainModels;

namespace SeeSql.DomainServices
{
    public class AutoMapperDataMapper : IDataMapper
    {
        private readonly IMapper _mapper;

        public AutoMapperDataMapper()
        {
            _mapper = new Mapper(new MapperConfiguration(expression =>
            {
                expression.CreateMap<sysjob, Job>();
                expression.CreateMap<sysjobstep, JobHistory>();
                expression.CreateMap<sysschedule, SystemSchedule>();
                expression.CreateMap<sysjobschedule, SystemJobSchedule>();
            }));
        }

        public T Map<T>(object source)
        {
            return _mapper.Map<T>(source);
        }

        public TDestination Project<TSource, TDestination>(TSource source, TDestination destination)
        {
            return _mapper.Map(source, destination);
        }
    }
}