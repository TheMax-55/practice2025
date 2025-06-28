using Xunit;
using Moq;
using task04;

public class SpaceshipTests
{
    [Fact]
    public void Cruiser_ShouldHaveCorrectStats()
    {
        ISpaceship cruiser = new Cruiser();
        Assert.Equal(50, cruiser.Speed);
        Assert.Equal(100, cruiser.FirePower);
    }

    [Fact]
    public void Fighter_ShouldHaveCorrectStats()
    {
        ISpaceship fighter = new Fighter();
        Assert.Equal(100, fighter.Speed);
        Assert.Equal(50, fighter.FirePower);
    }

    [Fact]
    public void Fighter_ShouldBeFasterThanCruiser()
    {
        var fighter = new Fighter();
        var cruiser = new Cruiser();
        Assert.True(fighter.Speed > cruiser.Speed);
    }

    [Fact]
    public void Cruiser_ShouldBePowerfulThanFighter()
    {
        var fighter = new Fighter();
        var cruiser = new Cruiser();
        Assert.True(cruiser.FirePower > fighter.FirePower);
    }

    [Fact]
    public void CruiserAndFighter_ShouldMoveCorrectly()
    {
        var fighter = new Fighter();
        var cruiser = new Cruiser();
        fighter.MoveForward();
        cruiser.MoveForward();
        Assert.Equal(100, fighter.Distance);
        Assert.Equal(50, cruiser.Distance);
    }

    [Fact]
    public void CruiserAndFighter_ShouldRotateCorrectly()
    {
        var fighter = new Fighter();
        var cruiser = new Cruiser();
        fighter.Rotate(30);
        cruiser.Rotate(90);
        Assert.Equal(30, fighter.Angle);
        Assert.Equal(90, cruiser.Angle);
    }

    [Fact]
    public void CruiserAndFighter_ShouldShootCorrectly()
    {
        var fighter = new Fighter();
        var cruiser = new Cruiser();
        fighter.Fire();
        cruiser.Fire();
        Assert.True(fighter.Shot);
        Assert.True(cruiser.Shot);
    }
}
