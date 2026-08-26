using System;
using ConferenceRoomBooking.Core.Services;
using Xunit;

namespace ConferenceRoomBooking.Tests;

public class PricingCalculatorTests
{
    private readonly PricingCalculator _calculator = new();

    [Fact]
    public void CalculateRoomCost_StandardHours_CalculatesBasePrice()
    {
        // Arrange
        decimal basePrice = 2000m;
        var start = new TimeOnly(9, 0);
        var end = new TimeOnly(11, 0); // 2 години стандартного часу (коеф. 1.0)

        // Act
        var cost = _calculator.CalculateRoomCost(basePrice, start, end);

        // Assert
        Assert.Equal(4000m, cost); // 2000 * 2 = 4000
    }

    [Fact]
    public void CalculateRoomCost_MorningHours_AppliesTenPercentDiscount()
    {
        // Arrange
        decimal basePrice = 2000m;
        var start = new TimeOnly(6, 0);
        var end = new TimeOnly(8, 0); // 2 години ранкового часу (-10%, коеф. 0.9)

        // Act
        var cost = _calculator.CalculateRoomCost(basePrice, start, end);

        // Assert
        Assert.Equal(3600m, cost); // 2000 * 2 * 0.9 = 3600
    }

    [Fact]
    public void CalculateRoomCost_PeakHours_AppliesFifteenPercentSurcharge()
    {
        // Arrange
        decimal basePrice = 2000m;
        var start = new TimeOnly(12, 0);
        var end = new TimeOnly(14, 0); // 2 години пікового часу (+15%, коеф. 1.15)

        // Act
        var cost = _calculator.CalculateRoomCost(basePrice, start, end);

        // Assert
        Assert.Equal(4600m, cost); // 2000 * 2 * 1.15 = 4600
    }

    [Fact]
    public void CalculateRoomCost_EveningHours_AppliesTwentyPercentDiscount()
    {
        // Arrange
        decimal basePrice = 2000m;
        var start = new TimeOnly(18, 0);
        var end = new TimeOnly(20, 0); // 2 години вечірнього часу (-20%, коеф. 0.8)

        // Act
        var cost = _calculator.CalculateRoomCost(basePrice, start, end);

        // Assert
        Assert.Equal(3200m, cost); // 2000 * 2 * 0.8 = 3200
    }

    [Fact]
    public void CalculateRoomCost_CrossingZones_CalculatesCorrectly()
    {
        // Arrange
        decimal basePrice = 2000m;
        var start = new TimeOnly(8, 0);  // 1 година ранку (8:00-9:00, -10% = 1800)
        var end = new TimeOnly(10, 0);  // 1 година стандарту (9:00-10:00, 1.0 = 2000)

        // Act
        var cost = _calculator.CalculateRoomCost(basePrice, start, end);

        // Assert
        Assert.Equal(3800m, cost); // 1800 + 2000 = 3800
    }

    [Fact]
    public void CalculateRoomCost_StartTimeAfterEndTime_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => 
            _calculator.CalculateRoomCost(2000m, new TimeOnly(12, 0), new TimeOnly(10, 0))
        );
    }

    [Fact]
    public void CalculateRoomCost_OutsideWorkingHours_ThrowsInvalidOperationException()
    {
        // Act & Assert (робота до 06:00 заборонена)
        Assert.Throws<InvalidOperationException>(() => 
            _calculator.CalculateRoomCost(2000m, new TimeOnly(5, 0), new TimeOnly(8, 0))
        );
    }
}