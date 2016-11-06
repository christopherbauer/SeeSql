using System.Reflection;
using System.Web.Mvc;
using AutoMapper;
using SeeSql.ApplicationServices;
using SeeSql.DomainServices;
using SimpleInjector;
using SimpleInjector.Integration.Web;

namespace SeeSql.WebApplication
{
    public class SimpleInjectorDependencyResolver
    {
        private static Container _container;

        public static void RegisterDependencyResolver(IRuntimeMapper mapper)
        {
            _container = new Container();
            _container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();
        
            _container.Register<MsdbEntities>(Lifestyle.Scoped);
            _container.Register<IJobDetailService, JobDetailService>();
            _container.Register<IJobRepository, JobRepository>();
            _container.Register<IDateTimeService,DateTimeService>();
            _container.Register<IDataMapper>(() => new DataMapper(mapper));

            mapper.DefaultContext.Options.ConstructServicesUsing(_container.GetInstance);

            _container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            _container.RegisterMvcIntegratedFilterProvider();

            _container.Verify();

            DependencyResolver.SetResolver(_container);
        }
    }
}