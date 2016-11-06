namespace SeeSql.DomainServices
{
    public interface IDataMapper
    {
        T Map<T>(object source);
        TDestination Project<TSource, TDestination>(TSource source, TDestination destination);
    }
}