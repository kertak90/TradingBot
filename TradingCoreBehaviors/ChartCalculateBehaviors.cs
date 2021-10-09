using FluentAssertions;
using Models;
using NUnit.Framework;
using TradingCore.Sundries;

namespace TradingCoreBehaviors
{
    public class ChartCalculateBehaviors
    {
        private ChartCalculate _target;
        [SetUp]
        public void Setup()
        {
            _target = new ChartCalculate();
        }

        [Test]
        public void ShouldReturnTrueWhenInvokeCheckIntersectionOfSegments1()
        {
            // Given
            var section1 = new ChartValuePair[]
            {
                new ChartValuePair { X = -2, Y = 1 },
                new ChartValuePair { X = 2, Y = -1 },
            };
            var section2 = new ChartValuePair[]
            {
                new ChartValuePair { X = -2, Y = -1 },
                new ChartValuePair { X = 2, Y = 1 },
            };

            // When
            var intersect = _target.CheckIntersectionOfSegments(section1, section2);

            // Then
            intersect.Should().BeTrue();
        }
        [Test]
        public void ShouldReturnTrueWhenInvokeCheckIntersectionOfSegments2()
        {
            // Given
            var section1 = new ChartValuePair[]
            {
                new ChartValuePair { X = 1, Y = 6 },
                new ChartValuePair { X = 5, Y = 2 },
            };
            var section2 = new ChartValuePair[]
            {
                new ChartValuePair { X = 1, Y = 2 },
                new ChartValuePair { X = 5, Y = 6 },
            };

            // When
            var intersect = _target.CheckIntersectionOfSegments(section1, section2);

            // Then
            intersect.Should().BeTrue();
        }
        [Test]
        public void ShouldReturnFalseWhenInvokeCheckIntersectionOfSegments1()
        {
            // Given
            var section1 = new ChartValuePair[]
            {
                new ChartValuePair { X = -2, Y = 1 },
                new ChartValuePair { X = 2, Y = 1 },
            };
            var section2 = new ChartValuePair[]
            {
                new ChartValuePair { X = -2, Y = -1 },
                new ChartValuePair { X = 2, Y = 0 }
            };

            // When
            var intersect = _target.CheckIntersectionOfSegments(section1, section2);

            // Then
            intersect.Should().BeFalse();
        }
        [Test]
        public void ShouldReturnFalseWhenInvokeCheckIntersectionOfSegments2()
        {
            // Given
            var section1 = new ChartValuePair[]
            {
                new ChartValuePair { X = 1, Y = 6 },
                new ChartValuePair { X = 5, Y = 5 },
            };
            var section2 = new ChartValuePair[]
            {
                new ChartValuePair { X = 1, Y = 2 },
                new ChartValuePair { X = 5, Y = 3 }
            };

            // When
            var intersect = _target.CheckIntersectionOfSegments(section1, section2);

            // Then
            intersect.Should().BeFalse();
        }
    }
}