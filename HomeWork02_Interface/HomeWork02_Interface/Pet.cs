using System;

namespace HomeWork02_Interface.Properties
{
    public class Pet: IAnimal, IRunnable
    {
        public Pet()
        {
            MaxSpeed = 0;
            LifeDuration = 0;
        }
        public Pet(double maxSpeed, double lifeDuration)
        {
            MaxSpeed = maxSpeed;
            LifeDuration = lifeDuration;
        }
        public double LifeDuration { get; set; }
        public double MaxSpeed { get; set;}
        
        public virtual void Voice()
        {
            Console.WriteLine("Meow!");
        }
        public virtual void ShowInfo()
        {
            Console.WriteLine($"I am a {this.GetType().Name} and I live approximately {LifeDuration} years.");
        }

        public virtual void Run()
        {
            Console.WriteLine($"I can run up to {MaxSpeed} kilometers per hour!" );
        }
    }
}