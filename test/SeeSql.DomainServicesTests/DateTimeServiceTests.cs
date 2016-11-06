using System;
using NUnit.Framework;

namespace SeeSql.DomainServices.Tests
{
    public class DateTimeServiceTests
    {

        [TestFixture]
        public class WhenGetCurrentUtcDateTime
        {
            [Test]
            public void ThenShouldReturnCurrentDateGivenCalled()
            {
                // Arrange
                var service = new DateTimeService();
                var currentDate = DateTime.UtcNow;

                //Act
                var serviceDate = service.GetCurrentUtcDateTime();

                //Assert
                Assert.That(currentDate.Date, Is.EqualTo(serviceDate.Date));
            }
        }

    }
}