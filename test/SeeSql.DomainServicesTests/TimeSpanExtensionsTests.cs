using System;
using NUnit.Framework;

namespace SeeSql.DomainServices.Tests
{
    [TestFixture]
    public class TimeSpanExtensionsTests
    {
        [TestFixture]
        public class WhenIsEqualTo
        {
            [Test]
            public void ThenShouldReturnTrueGivenSameTimeSpan()
            {
                // Arrange
                var timespan1 = new TimeSpan(2, 3, 4);
                var timespan2 = new TimeSpan(2, 3, 4);

                //Act
                var result = timespan1.IsEqualTo(timespan2);

                //Assert
                Assert.That(result, Is.True);
            }

            [Test]
            public void ThenShouldReturnFalseGivenDifferentTimeSpan()
            {
                // Arrange
                var timespan1 = new TimeSpan(2, 3, 4);
                var timespan2 = new TimeSpan(2, 4, 4);

                //Act
                var result = timespan1.IsEqualTo(timespan2);

                //Assert
                Assert.That(result, Is.False);
            }
        }

        [TestFixture]
        public class WhenIsGreaterOrEqualTo
        {
            [Test]
            public void ThenShouldReturnTrueGivenSameTimeSpan()
            {
                // Arrange
                var timespan1 = new TimeSpan(2, 3, 4);
                var timespan2 = new TimeSpan(2, 3, 4);

                //Act
                var result = timespan1.IsGreaterThanOrEqualTo(timespan2);

                //Assert
                Assert.That(result, Is.True);
            }

            [Test]
            public void ThenShouldReturnTrueGivenIsGreaterThan()
            {
                // Arrange
                var timespan1 = new TimeSpan(2, 5, 4);
                var timespan2 = new TimeSpan(2, 4, 4);

                //Act
                var result = timespan1.IsGreaterThanOrEqualTo(timespan2);

                //Assert
                Assert.That(result, Is.True);
            }

            [Test]
            public void ThenShouldReturnFalseGivenIsLessThan()
            {
                // Arrange
                var timespan1 = new TimeSpan(2, 3, 4);
                var timespan2 = new TimeSpan(2, 4, 4);

                //Act
                var result = timespan1.IsGreaterThanOrEqualTo(timespan2);

                //Assert
                Assert.That(result, Is.False);
            }
        }

        [TestFixture]
        public class WhenIsGreaterThan
        {
            [Test]
            public void ThenShouldReturnFalseGivenSameTimeSpan()
            {
                // Arrange
                var timespan1 = new TimeSpan(2, 3, 4);
                var timespan2 = new TimeSpan(2, 3, 4);

                //Act
                var result = timespan1.IsGreaterThan(timespan2);

                //Assert
                Assert.That(result, Is.False);
            }

            [Test]
            public void ThenShouldReturnTrueGivenIsGreaterThan()
            {
                // Arrange
                var timespan1 = new TimeSpan(2, 5, 4);
                var timespan2 = new TimeSpan(2, 4, 4);

                //Act
                var result = timespan1.IsGreaterThan(timespan2);

                //Assert
                Assert.That(result, Is.True);
            }

            [Test]
            public void ThenShouldReturnFalseGivenIsLessThan()
            {
                // Arrange
                var timespan1 = new TimeSpan(2, 3, 4);
                var timespan2 = new TimeSpan(2, 4, 4);

                //Act
                var result = timespan1.IsGreaterThan(timespan2);

                //Assert
                Assert.That(result, Is.False);
            }
        }

        [TestFixture]
        public class WhenIsLessOrEqualTo
        {
            [Test]
            public void ThenShouldReturnTrueGivenSameTimeSpan()
            {
                // Arrange
                var timespan1 = new TimeSpan(2, 3, 4);
                var timespan2 = new TimeSpan(2, 3, 4);

                //Act
                var result = timespan1.IsLessThanOrEqualTo(timespan2);

                //Assert
                Assert.That(result, Is.True);
            }

            [Test]
            public void ThenShouldReturnFalseGivenIsGreaterThan()
            {
                // Arrange
                var timespan1 = new TimeSpan(2, 5, 4);
                var timespan2 = new TimeSpan(2, 4, 4);

                //Act
                var result = timespan1.IsLessThanOrEqualTo(timespan2);

                //Assert
                Assert.That(result, Is.False);
            }

            [Test]
            public void ThenShouldReturnTrueGivenIsLessThan()
            {
                // Arrange
                var timespan1 = new TimeSpan(2, 3, 4);
                var timespan2 = new TimeSpan(2, 4, 4);

                //Act
                var result = timespan1.IsLessThanOrEqualTo(timespan2);

                //Assert
                Assert.That(result, Is.True);
            }
        }

        [TestFixture]
        public class WhenIsLessThan
        {
            [Test]
            public void ThenShouldReturnFalseGivenSameTimeSpan()
            {
                // Arrange
                var timespan1 = new TimeSpan(2, 3, 4);
                var timespan2 = new TimeSpan(2, 3, 4);

                //Act
                var result = timespan1.IsLessThan(timespan2);

                //Assert
                Assert.That(result, Is.False);
            }

            [Test]
            public void ThenShouldReturnFalseGivenIsGreaterThan()
            {
                // Arrange
                var timespan1 = new TimeSpan(2, 5, 4);
                var timespan2 = new TimeSpan(2, 4, 4);

                //Act
                var result = timespan1.IsLessThan(timespan2);

                //Assert
                Assert.That(result, Is.False);
            }

            [Test]
            public void ThenShouldReturnTrueGivenIsLessThan()
            {
                // Arrange
                var timespan1 = new TimeSpan(2, 3, 4);
                var timespan2 = new TimeSpan(2, 4, 4);

                //Act
                var result = timespan1.IsLessThan(timespan2);

                //Assert
                Assert.That(result, Is.True);
            }
        }

    }

}