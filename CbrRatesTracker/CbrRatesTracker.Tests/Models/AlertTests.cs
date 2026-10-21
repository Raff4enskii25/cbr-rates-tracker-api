using System;
using System.Collections.Generic;
using System.Text;
using CbrRatesTracker.Models;
using CbrRatesTracker.Enums;

namespace CbrRatesTracker.Tests.Models
{
    public class AlertTests
    {
        [Theory]
        [InlineData(75.0, Direction.Above, "test@test.com")]
        [InlineData(50.0, Direction.Below, "test@test.com")]
        [InlineData(100.0, Direction.Above, null)]
        public void TestAlertCreation(decimal thresholdValue, Direction direction, string email)
        {
            //Arrange
            var currency = new Currency("TTT", 777, "Test Dollar");
            var alert = new Alert(currency, thresholdValue, direction, email);

            //Assert
            Assert.False(alert.IsTriggered);
            Assert.Null(alert.TriggeredAt);
            Assert.Equal(thresholdValue, alert.ThresholdValue);
            Assert.Equal(direction, alert.Direction);
            Assert.Equal(email, alert.Email);
        }

        [Fact]
        public void TestAlertIsTriggered()
        {
            //Arrange
            var currency = new Currency("TTT", 777, "Test Dollar");
            var alert = new Alert(currency, 75.0m, Direction.Above, null);
            var testDate = new DateOnly(2026, 10, 21);

            //Act
            alert.SetAlertIsTriggered(testDate);

            //Assert
            Assert.True(alert.IsTriggered);
            Assert.Equal(testDate, alert.TriggeredAt);
        }
    }
}
