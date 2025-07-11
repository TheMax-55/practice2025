using Xunit;
using task11;

public class CalculatorTests
{
    string code = @"
    using task11;

    public class Calculator : ICalculator
    {
        public int Add(int a, int b) => a + b;
        public int Minus(int a, int b) => a - b;
        public int Mul(int a, int b) => a * b;
        public int Div(int a, int b) => a / b;
    }";

    [Fact]
    public void Add_ShouldWorkCorrectly()
    {
        var calculator = CreateCalculator.Compile(code);
        Assert.Equal(12, calculator.Add(7, 5));
    }

    [Fact]
    public void Minus_ShouldWorkCorrectly()
    {
        var calculator = CreateCalculator.Compile(code);
        Assert.Equal(-5, calculator.Minus(5, 10));
    }

    [Fact]
    public void Mul_ShouldWorkCorrectly()
    {
        var calculator = CreateCalculator.Compile(code);
        Assert.Equal(-56, calculator.Mul(7, -8));
    }

    [Fact]
    public void Div_ShouldWorkCorrectly()
    {
        var calculator = CreateCalculator.Compile(code);
        Assert.Equal(5, calculator.Div(15, 3));
    }

    [Fact]
    public void DivByZero_ShouldThrowDivideByZeroException()
    {
        var calculator = CreateCalculator.Compile(code);
        Assert.Throws<DivideByZeroException>(() => calculator.Div(1, 0));
    }
}
