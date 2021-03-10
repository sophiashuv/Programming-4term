using System;
using HomeWork02_Interface.Properties;

namespace HomeWork02_Interface
{
    /*
    Визначити інтерфейс ISwimmable з методом Swim(), що за замовчуванням виводить на консоль "I can swim!".
    Визначити інтерфейс IFlyable з властивістю для читання MaxHeight, що за замовчуванням повертає 0 та з методом Fly(),
    що виводить текст "I can fly at XX meters height!" де XX значення властивості MaxHeight.
    Визначити інтерфейс IRunnable з властивістю для читання MaxSpeed що за замовчуванням повертає 0, та з методом Run(),
    що виводить текст "I can run up to XX kilometers per hour!" де XX - значення властивості MaxSpeed.
    Визначити інтерфейс IAnimal з властивістю для читання LifeDuration, що за замовчуванням повертає 0, та з методом
    Voice(), що виводить текст "No voice!". Крім того, визначити метод ShowInfo() що виводить текст
    "I am a XX and I live approximately YY years." де XX – назва класу, що реалізує інтерфейс, YY – значення властивості
    LifeDuration.
    Визначити клас Cat, що імплементує інтерфейси IAnimal і IRunnable. Реалізувати метод Voice(), що виводить "Meow!". 
    Визначити клас Eagle, що імплементує інтерфейси IAnimal та IFlyable , а також клас Shark, що імплементує інтерфейси 
    IAnimal і ISwimmable. В цих класах визначити відповідним чином властивості..
    Заповнити масив об’єктами цих класів та вивести інформацію про них за допомогою ShowInfo().
    */
    internal class Program
    {
        public static void Main(string[] args)
        {
            IAnimal[] elements = { 
                new Pet() { LifeDuration = 2, MaxSpeed = 3}, 
                new Shark() { LifeDuration = 5}, 
                new Eagle() { LifeDuration = 4, MaxHeight = 3 }};
            
            foreach (IAnimal x in elements)
            {
                x.ShowInfo();
            }
        }
    }
}