using System;
using NUnit.Framework;

namespace SeeSql.WebApplication.Tests
{
    public class DateTimeExtensionTests
    {
        public class WhenToJavascriptString
        {
            [Test]
            [TestCase(1)]
            [TestCase(2)]
            [TestCase(3)]
            [TestCase(4)]
            [TestCase(5)]
            [TestCase(6)]
            [TestCase(7)]
            [TestCase(8)]
            [TestCase(9)]
            [TestCase(10)]
            [TestCase(11)]
            [TestCase(12)]
            public void ThenShouldReturnMonthMinus1GivenAnyMonth(int realMonth)
            {
                // Arrange
                var date = new DateTime(2016, realMonth, 22, 12, 0, 0);

                //Act
                var javascriptStringSplit = date.ToJavascriptString().Split(',');
                var javascriptMonth = javascriptStringSplit[1];

                //Assert
                Assert.That(javascriptMonth, Is.EqualTo((realMonth - 1).ToString()));
            }

            [Test]
            public void ThenShouldReturnMilitaryTimeGiven17InHours()
            {
                // Arrange
                var date = new DateTime(2016, 8, 22, 17, 40, 01);

                //Act
                var javascriptString = date.ToJavascriptString();
                var javascriptHour = javascriptString.Split(',')[3];

                //Assert
                Assert.That(javascriptHour, Is.EqualTo("17"));
            }

            [Test]
            public void ThenShouldReturnMilitaryTimeGiven5InHours()
            {
                // Arrange
                var date = new DateTime(2016, 8, 22, 5, 40, 01);

                //Act
                var javascriptString = date.ToJavascriptString();
                var javascriptHour = javascriptString.Split(',')[3];

                //Assert
                Assert.That(javascriptHour, Is.EqualTo("5"));
            }
        }


    }
}