using CalculatorTest.Interfaces;
using CalculatorTest.Services;
using Shouldly;

namespace TestProject1;

public class Tests
{
    private ISimpleCalculator _simpleCalculator;

    private const int NUMBER_1 = 10;
    private const int NUMBER_2 = -4;
    private const int NUMBER_3 = int.MaxValue;
    private const int NUMBER_4 = int.MinValue;

    private const int EXPECTED_ADDITION = 6;
    private const int EXPECTED_SUBTRACTION = 14;
    
    [SetUp]
    public void Setup()
    {
        _simpleCalculator = new SimpleCalculator();
    }

    [Test]
    public void ShouldAddPositiveAndNegative()
    {
        var result = _simpleCalculator.Add(NUMBER_1, NUMBER_2);
        result.ShouldBe(EXPECTED_ADDITION);
    }

    [Test]
    public void ShouldSubtractPositiveAndNegative()
    {
        var result = _simpleCalculator.Subtract(NUMBER_1, NUMBER_2);
        result.ShouldBe(EXPECTED_SUBTRACTION);
    }
    
    [Test]
    public void ShouldThrowWhenAddOverflows()
    {
        Should.Throw<OverflowException>(() => _simpleCalculator.Add(NUMBER_1, NUMBER_3));
    }

    [Test]
    public void ShouldThrowWhenSubtractUnderflows()
    {
        Should.Throw<OverflowException>(() => _simpleCalculator.Subtract(NUMBER_4, 1));
    }
}
