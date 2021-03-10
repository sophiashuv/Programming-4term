using System;

namespace HomeWork02_Interface
{
    public class Eagle: IAnimal, IFlyable
    {
        public Eagle(double lifeDuration, double maxHeight)
        {
            LifeDuration = lifeDuration;
            MaxHeight = maxHeight;
        }
        
        public Eagle()
        {
            LifeDuration = 0;
            MaxHeight = 0;
        }

        public double LifeDuration { get; set; }
        public double MaxHeight { get; set;}
        
        public virtual void Voice()
        {
            throw new System.NotImplementedException();
        }

        public virtual void ShowInfo()
        {
            Console.WriteLine($"I am a {this.GetType().Name} and I live approximately {LifeDuration} years.");
        }

        public virtual void Fly()
        {
            Console.WriteLine($"I can fly at {MaxHeight} meters height!");
        }
    }
}