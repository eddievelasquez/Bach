using System;
using Xunit;
using FluentAssertions;
using Bach.Model;

namespace Bach.Model.Test
{
    public class IntervalQualityAdditionalTests
    {
        public static TheoryData<IntervalQuality, string> SymbolData => new()
        {
            { IntervalQuality.Diminished, "d" },
            { IntervalQuality.Minor, "m" },
            { IntervalQuality.Perfect, "P" },
            { IntervalQuality.Major, "M" },
            { IntervalQuality.Augmented, "A" }
        };

        [Theory]
        [MemberData(nameof(SymbolData))]
        public void Symbol_QualityProvided_ReturnsExpected(IntervalQuality quality, string expected)
        {
            // Arrange & Act
            var result = quality.Symbol;

            // Assert
            result.Should().Be(expected);
        }

        public static TheoryData<IntervalQuality, string> ShortNameData => new()
        {
            { IntervalQuality.Diminished, "dim" },
            { IntervalQuality.Minor, "min" },
            { IntervalQuality.Perfect, "Perf" },
            { IntervalQuality.Major, "Maj" },
            { IntervalQuality.Augmented, "Aug" }
        };

        [Theory]
        [MemberData(nameof(ShortNameData))]
        public void ShortName_QualityProvided_ReturnsExpected(IntervalQuality quality, string expected)
        {
            // Act
            var result = quality.ShortName;

            // Assert
            result.Should().Be(expected);
        }

        public static TheoryData<IntervalQuality, string> LongNameData => new()
        {
            { IntervalQuality.Diminished, "Diminished" },
            { IntervalQuality.Minor, "Minor" },
            { IntervalQuality.Perfect, "Perfect" },
            { IntervalQuality.Major, "Major" },
            { IntervalQuality.Augmented, "Augmented" }
        };

        [Theory]
        [MemberData(nameof(LongNameData))]
        public void LongName_QualityProvided_ReturnsExpected(IntervalQuality quality, string expected)
        {
            // Act
            var result = quality.LongName;

            // Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void Add_WithPositiveSemitones_ReturnsExpectedQuality()
        {
            // Arrange
            var start = IntervalQuality.Minor; // value = 1

            // Act
            var result = start.Add(2); // 1 + 2 = 3 -> Major

            // Assert
            result.Should().Be(IntervalQuality.Major);
        }

        [Fact]
        public void Add_WithNegativeSemitones_ReturnsExpectedQuality()
        {
            // Arrange
            var start = IntervalQuality.Major; // value = 3

            // Act
            var result = start.Add(-2); // 3 - 2 = 1 -> Minor

            // Assert
            result.Should().Be(IntervalQuality.Minor);
        }

        public static TheoryData<IntervalQuality, int> AddOutOfRangeData => new()
        {
            { IntervalQuality.Augmented, 1 },
            { IntervalQuality.Diminished, -1 }
        };

        [Theory]
        [MemberData(nameof(AddOutOfRangeData))]
        public void Add_ResultOutOfRange_ThrowsArgumentOutOfRange(IntervalQuality quality, int semitones)
        {
            // Act
            Action act = () => quality.Add(semitones);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void CompareTo_ObjectNull_ReturnsOne()
        {
            // Arrange
            var quality = IntervalQuality.Major;

            // Act
            var result = quality.CompareTo((object?)null);

            // Assert
            result.Should().Be(1);
        }

        public static TheoryData<IntervalQuality, object, int> CompareToCases => new()
        {
            { IntervalQuality.Major, (object)IntervalQuality.Minor, 1 },
            { IntervalQuality.Perfect, (object)IntervalQuality.Perfect, 0 },
            { IntervalQuality.Minor, (object)IntervalQuality.Augmented, -1 }
        };

        [Theory]
        [MemberData(nameof(CompareToCases))]
        public void CompareTo_ObjectInterval_ReturnsExpectedComparison(IntervalQuality left, object right, int expectedSign)
        {
            // Act
            var result = left.CompareTo(right);

            // Assert: only check sign (-1,0,1)
            Math.Sign(result).Should().Be(Math.Sign(expectedSign));
        }

        [Fact]
        public void CompareTo_ObjectWrongType_ThrowsArgumentException()
        {
            // Arrange
            var quality = IntervalQuality.Perfect;
            var wrong = "not an interval";

            // Act
            Action act = () => quality.CompareTo(wrong);

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("Object must be of type IntervalQuality");
        }
    }
}
