using System;
using System.Collections;
namespace task04
{
    public interface ISpaceship
    {
        void MoveForward();
        void Rotate(int angle);
        void Fire();
        int Speed { get; }
        int FirePower { get; }
    }
    public class Cruiser : ISpaceship
    {
        public int Distance;
        public int Angle;
        public bool Shot;
        public int Speed { get; } = 50;
        public int FirePower { get; } = 100;
        public void MoveForward() => Distance += Speed;
        public void Rotate(int angle) => Angle = (Angle + angle) % 360;
        public void Fire() => Shot = true; 
        
    }
    public class Fighter : ISpaceship
    {
        public int Distance;
        public int Angle;
        public bool Shot;
        public int Speed { get; } = 100;
        public int FirePower { get; } = 50;
        public void MoveForward() => Distance += Speed;
        public void Rotate(int angle) => Angle = (Angle + angle) % 360;
        public void Fire() => Shot = true; 
    }
}
