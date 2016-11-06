using System;
using AutoMapper;
using SeeSql.ApplicationServices;
using SeeSql.DomainModels;
using SeeSql.DomainServices;

namespace SeeSql.WebApplication
{
    public class AutoMapperConfig
    {
        public static Mapper RegisterMappings(Func<Type, object> serviceConstructor)
        {
            var mapperConfiguration = new MapperConfiguration(mapper =>
            {
                mapper.ConstructServicesUsing(serviceConstructor);
                mapper.SourceMemberNamingConvention = new LowerUnderscoreNamingConvention();

                mapper.CreateMap<sysjob, Job>();
                mapper.CreateMap<sysjobstep, JobStep>();
                mapper.CreateMap<sysjobhistory, JobHistory>()
                    .ForMember(history => history.RunDate,
                        expression => expression.MapFrom(sysjobhistory => GetNullableDateTime(SqlConverter.GetDateTimeFromInt(sysjobhistory.run_date, sysjobhistory.run_time, DateTimeKind.Utc))))
                    .ForMember(history => history.RunDuration,
                        expression => expression.MapFrom(sysjobhistory => SqlConverter.GetRunDurationFromInt(sysjobhistory.run_duration)));
                mapper.CreateMap<sysschedule, SystemSchedule>()
                    .ForMember(schedule => schedule.ActiveStartDate,
                        expression =>
                            expression.MapFrom(
                                sysschedule =>
                                    GetNullableDateTime(SqlConverter.GetDateTimeFromInt(sysschedule.active_start_date,
                                        sysschedule.active_start_time, DateTimeKind.Utc))))
                    .ForMember(schedule => schedule.ActiveEndDate,
                        expression =>
                            expression.MapFrom(
                                sysschedule =>
                                    GetNullableDateTime(SqlConverter.GetDateTimeFromInt(sysschedule.active_end_date,
                                        sysschedule.active_end_time, DateTimeKind.Utc))))
                    .ForMember(systemSchedule => systemSchedule.Enabled,
                        expression => expression.MapFrom(sysschedule => sysschedule.enabled == 1));
                mapper.CreateMap<sysjobschedule, SystemJobSchedule>().ForMember(schedule => schedule.NextRunDate,
                        expression =>
                            expression.MapFrom(
                                sysschedule =>
                                    GetNullableDateTime(SqlConverter.GetDateTimeFromInt(sysschedule.next_run_date,
                                        sysschedule.next_run_time, DateTimeKind.Utc))));
                mapper.CreateMap<SystemJobSchedule, JobSchedule>();
                mapper.CreateMap<SystemSchedule, JobSchedule>();
//                mapper.CreateMap<sysjobactivity, RunningJobActivity>();
//                mapper.CreateMap<Job, RunningJobActivity>();

            });
            
            return new Mapper(mapperConfiguration);
        }

        private static DateTime? GetNullableDateTime(DateTime? target)
        {
            return target.HasValue ? target.Value.ToLocalTime() : target;
        }
    }
}
