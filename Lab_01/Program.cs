using System;

namespace Lab_01
{
    class Program
    {
        static void Main(string[] args) 
        {
            Person sofia = new Person("Sofia", "Shuvar", Sex.female);
            Person Zainab = new Person("Zainab", "Cote", Sex.male);
            Person Rubi = new Person("Rubi", "Mair", Sex.female);

            Person[] people = new Person[]{sofia, Zainab, Rubi};

            foreach (var person in people)
            {
                Console.WriteLine(person);
            }

        }
    }
}

    