using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using SeeSql.ApplicationServices;
using SeeSql.DomainModels;

namespace SeeSql.DomainServices.Tests
{
    [TestFixture]
    public class JobDetailServiceTests
    {
        [TestFixture]
        public class WhenAct
        {
            [Test]
            //All cases should perform the same essential interpolation because they mean the same period
            //M-F 12a-125959p
            [TestCase(8, 1, "2016-08-29", "2016-09-04 23:59:59", 167, "12a - 125959p Every 1 hours")]
            [TestCase(8, 2, "2016-08-29", "2016-09-04 23:59:59", 83, "12a - 125959p Every 2 hours")]
            [TestCase(4, 60, "2016-08-29", "2016-09-04 23:59:59", 167, "12a - 125959p Every 60 minutes")]
            [TestCase(4, 15, "2016-08-29", "2016-09-04 23:59:59", 671, "12a - 125959p Every 15 minutes")]
            [TestCase(2, 3600, "2016-08-29", "2016-09-04 23:59:59", 167, "12a - 125959p Every 3600 seconds")]
            [TestCase(1, 0, "2016-08-29", "2016-09-04 23:59:59", 7, "12a - 125959p At the specified time")]
            //M-F 8a-5p
            [TestCase(8, 1, "2016-08-29 08:00:00", "2016-09-04 17:00:00", 70, "8a - 5p Every 1 hours")]
            [TestCase(4, 60, "2016-08-29 08:00:00", "2016-09-04 17:00:00", 70, "8a - 5p Every 60 minutes")]
            [TestCase(4, 30, "2016-08-29 08:00:00", "2016-09-04 17:00:00", 133, "8a - 5p Every 30 minutes")]
            [TestCase(2, 3600, "2016-08-29 08:00:00", "2016-09-04 17:00:00", 70, "8a - 5p Every 3600 seconds")]
            [TestCase(1, 0, "2016-08-29 08:00:00", "2016-09-04 17:00:00", 7, "8a - 5p At the specified time")]
            public void ThenShouldReturnExpectedCountExecutionsGivenRangeStart000000AndRangeEnd125959(
                int frequencySubdayType, int frequencySubdayInterval, string activeStartDateTime,
                string activeEndDateTime, int expectedCount, string description)
            {
                // Arrange
                var jobRepository = new Mock<IJobRepository>();
                var jobId = new Guid("C647117A-050A-4913-8BD8-D92A20315351");
                var scheduleId = 1;
                var sysjobschedules = new List<sysjobschedule>
                {
                    new sysjobschedule {job_id = jobId, schedule_id = scheduleId}
                };
                jobRepository.Setup(repository => repository.GetJobSchedules()).Returns(sysjobschedules);

                var sysschedules = new List<sysschedule> { new sysschedule() };
                //doesn't really matter what values are here for testing purposes, as the object gets datamapper-d into its final form
                jobRepository.Setup(repository => repository.GetSchedules())
                    .Returns(sysschedules);

                var jobs = new List<Job> { new Job { JobId = jobId } };
                jobRepository.Setup(repository => repository.GetJobHistory(It.IsAny<IEnumerable<Guid>>()))
                    .Returns(new List<sysjobhistory> { new sysjobhistory { job_id = jobId } });

                var dataMapper = new Mock<IDataMapper>();

                dataMapper.Setup(mapper => mapper.Map<IList<SystemJobSchedule>>(sysjobschedules))
                    .Returns(new List<SystemJobSchedule>
                    {
                        new SystemJobSchedule
                        {
                            JobId = jobId,
                            ScheduleId = scheduleId,
                            NextRunDate = new DateTime(2016, 8, 29)
                        }
                    });

                var activeStartDate = DateTime.Parse(activeStartDateTime);
                var activeEndDate = DateTime.Parse(activeEndDateTime);
                dataMapper.Setup(mapper => mapper.Map<IList<SystemSchedule>>(sysschedules))
                    .Returns(new List<SystemSchedule>
                    {
                        new SystemSchedule
                        {
                            ScheduleId = scheduleId,
                            FreqType = 4, //Daily
                            FreqInterval = 62, //M-F
                            ActiveStartDate = activeStartDate,
                            ActiveEndDate = activeEndDate,
                            FreqSubdayType = frequencySubdayType,
                            FreqSubdayInterval = frequencySubdayInterval
                        }
                    });

                var jobDetailService = new JobDetailService(dataMapper.Object, jobRepository.Object);

                //Act
                var result = jobDetailService.GetInterpolatedRunTimes(jobs,
                    new DateTime(2016, 8, 29),
                    new DateTime(2016, 9, 4, 23, 59, 59));

                //Assert
                Assert.That(result, Has.Count.EqualTo(expectedCount), description);
                Assert.That(result,
                        Has.All.Matches<ScheduledExecution>(
                            execution =>
                                execution.ExpectedRunDate.TimeOfDay.IsGreaterThanOrEqualTo(activeStartDate.TimeOfDay) &&
                                //Expected run date times are not before the start date time
                                execution.ExpectedRunDate.TimeOfDay.IsLessThanOrEqualTo(activeEndDate.TimeOfDay)));
                //Expected run date times are not after the end date time
            }
        }

        [TestFixture]
        public class WhenGetInterpolatedRunTimeFreqType8AndFreqInterval62
        //Freq Type 8 is Weekly, Freq Interval 62 means Monday, Tuesday, Wednesday, Thursday, Friday
        {
            [Test]
            //All cases should perform the same essential interpolation because they mean the same period
            //M-F 12a-125959p
            [TestCase(8, 1, "2016-08-29", "2016-09-04 23:59:59", 119, "12a - 125959p Every 1 hours")]
            [TestCase(8, 2, "2016-08-29", "2016-09-04 23:59:59", 59, "12a - 125959p Every 2 hours")]
            [TestCase(4, 60, "2016-08-29", "2016-09-04 23:59:59", 119, "12a - 125959p Every 60 minutes")]
            [TestCase(4, 15, "2016-08-29", "2016-09-04 23:59:59", 479, "12a - 125959p Every 15 minutes")]
            [TestCase(2, 3600, "2016-08-29", "2016-09-04 23:59:59", 119, "12a - 125959p Every 3600 seconds")]
            [TestCase(1, 0, "2016-08-29", "2016-09-04 23:59:59", 5, "12a - 125959p At the specified time")]
            //M-F 8a-5p
            [TestCase(8, 1, "2016-08-29 08:00:00", "2016-09-04 17:00:00", 50, "8a - 5p Every 1 hours")]
            [TestCase(4, 60, "2016-08-29 08:00:00", "2016-09-04 17:00:00", 50, "8a - 5p Every 60 minutes")]
            [TestCase(4, 30, "2016-08-29 08:00:00", "2016-09-04 17:00:00", 95, "8a - 5p Every 30 minutes")]
            [TestCase(2, 3600, "2016-08-29 08:00:00", "2016-09-04 17:00:00", 50, "8a - 5p Every 3600 seconds")]
            [TestCase(1, 0, "2016-08-29 08:00:00", "2016-09-04 17:00:00", 5, "8a - 5p At the specified time")]
            public void ThenShouldReturnExpectedCountExecutionsGivenRangeStart000000AndRangeEnd125959(
                int frequencySubdayType, int frequencySubdayInterval, string activeStartDateTime,
                string activeEndDateTime, int expectedCount, string description)
            {
                // Arrange
                var jobRepository = new Mock<IJobRepository>();
                var jobId = new Guid("C647117A-050A-4913-8BD8-D92A20315351");
                var scheduleId = 1;
                var sysjobschedules = new List<sysjobschedule>
                {
                    new sysjobschedule {job_id = jobId, schedule_id = scheduleId}
                };
                jobRepository.Setup(repository => repository.GetJobSchedules()).Returns(sysjobschedules);

                var sysschedules = new List<sysschedule> { new sysschedule() };
                //doesn't really matter what values are here for testing purposes, as the object gets datamapper-d into its final form
                jobRepository.Setup(repository => repository.GetSchedules())
                    .Returns(sysschedules);

                var jobs = new List<Job> { new Job { JobId = jobId } };
                jobRepository.Setup(repository => repository.GetJobHistory(It.IsAny<IEnumerable<Guid>>()))
                    .Returns(new List<sysjobhistory> { new sysjobhistory { job_id = jobId } });

                var dataMapper = new Mock<IDataMapper>();

                dataMapper.Setup(mapper => mapper.Map<IList<SystemJobSchedule>>(sysjobschedules))
                    .Returns(new List<SystemJobSchedule>
                    {
                        new SystemJobSchedule
                        {
                            JobId = jobId,
                            ScheduleId = scheduleId,
                            NextRunDate = new DateTime(2016, 8, 29)
                        }
                    });

                var activeStartDate = DateTime.Parse(activeStartDateTime);
                var activeEndDate = DateTime.Parse(activeEndDateTime);
                dataMapper.Setup(mapper => mapper.Map<IList<SystemSchedule>>(sysschedules))
                    .Returns(new List<SystemSchedule>
                    {
                        new SystemSchedule
                        {
                            ScheduleId = scheduleId,
                            FreqType = 8, //Weekly
                            FreqInterval = 62, //M-F
                            ActiveStartDate = activeStartDate,
                            ActiveEndDate = activeEndDate,
                            FreqSubdayType = frequencySubdayType,
                            FreqSubdayInterval = frequencySubdayInterval
                        }
                    });

                var jobDetailService = new JobDetailService(dataMapper.Object, jobRepository.Object);

                //Act
                var result = jobDetailService.GetInterpolatedRunTimes(jobs,
                    new DateTime(2016, 8, 29),
                    new DateTime(2016, 9, 4, 23, 59, 59));

                //Assert
                Assert.That(result, Has.Count.EqualTo(expectedCount), description);
                Assert.That(result,
                        Has.All.Matches<ScheduledExecution>(
                            execution =>
                                execution.ExpectedRunDate.TimeOfDay.IsGreaterThanOrEqualTo(activeStartDate.TimeOfDay) &&
                                //Expected run date times are not before the start date time
                                execution.ExpectedRunDate.TimeOfDay.IsLessThanOrEqualTo(activeEndDate.TimeOfDay)));
                //Expected run date times are not after the end date time
            }

            [Test]
            //All cases should perform the same essential interpolation because they mean the same period
            //M-F 12a-125959p
            [TestCase(8, 1, "2016-08-29", "2016-09-04 23:59:59", 102, "12a - 125959p Every 1 hours")]
            [TestCase(4, 60, "2016-08-29", "2016-09-04 23:59:59", 102, "12a - 125959p Every 60 minutes")]
            [TestCase(2, 3600, "2016-08-29", "2016-09-04 23:59:59", 102, "12a - 125959p Every 3600 seconds")]
            [TestCase(1, 0, "2016-08-29", "2016-09-04 23:59:59", 5, "12a - 125959p At the specified time")]
            //M-F 8a-5p
            [TestCase(8, 1, "2016-08-29 08:00:00", "2016-09-04 17:00:00", 46, "8a - 5p Every 1 hours")]
            [TestCase(4, 60, "2016-08-29 08:00:00", "2016-09-04 17:00:00", 46, "8a - 5p Every 60 minutes")]
            [TestCase(2, 3600, "2016-08-29 08:00:00", "2016-09-04 17:00:00", 46, "8a - 5p Every 3600 seconds")]
            [TestCase(1, 0, "2016-08-29 08:00:00", "2016-09-04 17:00:00", 5, "8a - 5p At the specified time")]
            public void ThenShouldReturnExpectedCountExecutionsGivenRangeStart070000AndRangeEnd140000(
                int frequencySubdayType, int frequencySubdayInterval, string activeStartDateTime,
                string activeEndDateTime, int expectedCount, string description)
            {
                // Arrange
                var jobRepository = new Mock<IJobRepository>();
                var jobId = new Guid("C647117A-050A-4913-8BD8-D92A20315351");
                var scheduleId = 1;
                var sysjobschedules = new List<sysjobschedule>
                {
                    new sysjobschedule {job_id = jobId, schedule_id = scheduleId}
                };
                jobRepository.Setup(repository => repository.GetJobSchedules()).Returns(sysjobschedules);

                var sysschedules = new List<sysschedule> { new sysschedule() };
                //doesn't really matter what values are here for testing purposes, as the object gets datamapper-d into its final form
                jobRepository.Setup(repository => repository.GetSchedules())
                    .Returns(sysschedules);

                var jobs = new List<Job> { new Job { JobId = jobId } };
                jobRepository.Setup(repository => repository.GetJobHistory(It.IsAny<IEnumerable<Guid>>()))
                    .Returns(new List<sysjobhistory> { new sysjobhistory { job_id = jobId } });

                var dataMapper = new Mock<IDataMapper>();

                dataMapper.Setup(mapper => mapper.Map<IList<SystemJobSchedule>>(sysjobschedules))
                    .Returns(new List<SystemJobSchedule>
                    {
                        new SystemJobSchedule
                        {
                            JobId = jobId,
                            ScheduleId = scheduleId,
                            NextRunDate = new DateTime(2016, 8, 29, 8, 0, 0)
                        }
                    });

                var activeStartDate = DateTime.Parse(activeStartDateTime);
                var activeEndDate = DateTime.Parse(activeEndDateTime);
                dataMapper.Setup(mapper => mapper.Map<IList<SystemSchedule>>(sysschedules))
                    .Returns(new List<SystemSchedule>
                    {
                        new SystemSchedule
                        {
                            ScheduleId = scheduleId,
                            FreqType = 8, //Weekly
                            FreqInterval = 62, //M-F
                            ActiveStartDate = activeStartDate,
                            ActiveEndDate = activeEndDate,
                            FreqSubdayType = frequencySubdayType,
                            FreqSubdayInterval = frequencySubdayInterval
                        }
                    });


                var jobDetailService = new JobDetailService(dataMapper.Object, jobRepository.Object);

                //Act
                var result = jobDetailService.GetInterpolatedRunTimes(jobs,
                    new DateTime(2016, 8, 29, 7, 0, 0),
                    new DateTime(2016, 9, 2, 14, 0, 0));

                //Assert
                Assert.That(result, Has.Count.EqualTo(expectedCount), description);
                Assert.That(result,
                        Has.All.Matches<ScheduledExecution>(
                            execution =>
                                execution.ExpectedRunDate.TimeOfDay.IsGreaterThanOrEqualTo(activeStartDate.TimeOfDay) &&
                                //Expected run date times are not before the start date time
                                execution.ExpectedRunDate.TimeOfDay.IsLessThanOrEqualTo(activeEndDate.TimeOfDay)));
                //Expected run date times are not after the end date time
            }
        }
    }
}