using AutoMapper;
using SeeSql.DomainServices;

namespace SeeSql.WebApplication
{
    public class DataMapper : IDataMapper
    {
        private readonly IMapper _mapper;

        public DataMapper(IMapper mapper)
        {
            _mapper = mapper;
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