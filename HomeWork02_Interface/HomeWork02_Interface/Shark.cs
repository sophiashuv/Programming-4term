using System;

namespace HomeWork02_Interface
{
    public class Shark: IAnimal, ISwimmable
    {
        public Shark(double lifeDuration)
        {
            LifeDuration = lifeDuration;
        }
        
        public Shark()
        {
            LifeDuration = 0;
        }

        public double LifeDuration { get; set; }
        public virtual void Voice()
        {
            Console.WriteLine("No voice!");
        }

        public virtual void ShowInfo()
        {
            Console.WriteLine($"I am a {this.GetType().Name} and I live approximately {LifeDuration} years.");
        }

        public virtual void Swim()
        {
            Console.WriteLine("I can swim!");
        }
    }
}