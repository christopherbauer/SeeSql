using System.Reflection;
using System.Web.Mvc;
using AutoMapper;
using SeeSql.ApplicationServices;
using SeeSql.DomainServices;
using SeeSql.WebApplication;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;

[assembly: WebActivator.PostApplicationStartMethod(typeof(SimpleInjectorInitializer), "Initialize")]

namespace SeeSql.WebApplication
{
    public static class SimpleInjectorInitializer
    {
        /// <summary>Initialize the container and register it as MVC3 Dependency Resolver.</summary>
        public static Container Initialize()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();
            
            InitializeContainer(container);

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));

            return container;
        }
     
        private static void InitializeContainer(Container container)
        {
            container.Register(() => new MsdbEntities(), Lifestyle.Scoped);
            container.Register<IDateTimeService, DateTimeService>();
            container.Register<IJobDetailService, JobDetailService>();
            container.Register<IJobRepository, JobRepository>();
            container.Register<IMapper>(() => AutoMapperConfig.RegisterMappings(container.GetInstance));
            container.Register<IDataMapper, DataMapper>();

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            container.Verify(VerificationOption.VerifyAndDiagnose);

        }
    }
}