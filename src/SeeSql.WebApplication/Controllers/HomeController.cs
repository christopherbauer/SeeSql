using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using SeeSql.DomainServices;
using SeeSql.WebApplication.ViewModels;

namespace SeeSql.WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly IJobDetailService _jobDetailService;
        private readonly IDateTimeService _dateTimeService;

        public HomeController(IJobDetailService jobDetailService, IDateTimeService dateTimeService)
        {
            _jobDetailService = jobDetailService;
            _dateTimeService = dateTimeService;
        }

        public ActionResult Index()
        {
            return View(new DashboardViewModel
            {
                CurrentServerTime = _dateTimeService.GetCurrentDateTime(),
                CurrentUniversalTime = _dateTimeService.GetCurrentUtcDateTime()
            });
        }

        public ActionResult JobList()
        {
            var jobs = _jobDetailService.GetJobs();
            return PartialView(new JobListViewModel { DataTableName = "JobList", Jobs = jobs });
        }

        public ActionResult JobSchedules()
        {
            var jobSchedules = _jobDetailService.GetJobSchedules();
            var jobs = _jobDetailService.GetJobs();
            return PartialView(new JobScheduleListViewModel
            {
                JobSchedules = jobSchedules,
                Jobs = jobs
            });
        }

        public ActionResult PlannedExecutions()
        {
            var jobSchedules = _jobDetailService.GetJobSchedules();
            var jobs = _jobDetailService.GetJobs();
            var currentDateTime = _dateTimeService.GetCurrentDateTime();
            var rangeStart = currentDateTime.AddHours(-1);
            var rangeEnd = jobSchedules.Max(schedule => schedule.NextRunDate).AddHours(1);
            var interpolatedRunTimes = _jobDetailService.GetInterpolatedRunTimes(jobs, rangeStart, rangeEnd).ToList();
            var jobRunTimeStatistics = _jobDetailService.GetRunTimeStatistics(jobs, currentDateTime);

            return PartialView(new JobSchedulerListViewModel
            {
                JobSchedules = jobSchedules,
                Jobs = jobs,
                JobRunTimeStatistics = jobRunTimeStatistics,
                InterpolatedRunTimes = interpolatedRunTimes,
                JobCount = interpolatedRunTimes.DistinctBy(run => run.JobId).Count(),
                CurrentDateTime = currentDateTime,
                RangeStart = rangeStart,
                RangeEnd = rangeEnd
            });

        }


        public PartialViewResult JobHistoryRollup(Guid jobId)
        {
            var jobHistoryRollup = _jobDetailService.GetJobHistoryRollup(jobId);
            jobHistoryRollup = jobHistoryRollup.OrderByDescending(history => history.RunDate).ToList();

            return PartialView(new JobHistoryViewModel
            {
                JobId = jobId,
                JobHistory = jobHistoryRollup
            });
        }

        public PartialViewResult JobStepCode(Guid jobId, int stepId)
        {
            var code = _jobDetailService.GetJobStep(jobId, stepId).Command;
            return PartialView("SqlCode", code);
        }

        public PartialViewResult Loading()
        {
            return PartialView("Loading");
        }
    }
}