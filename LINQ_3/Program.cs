using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;

namespace LINQ_3
{
    internal class Program
    {
        // Завдання 1.Розробити типи для облiку видачi пацiєнтам лiкiв в медзакладi вiдпо-вiдно
        // до призначень лiкаря. Пацiєнт характеризується iменем i прiзвищемта реєстрацiйним 
        // номером. Призначення характеризується реєстрацiйнимномером пацiєнта, датою, 
        // назвою лiкiв, дозою в мг i кiлькiстю прийомiву день. Iнформацiя про пацiєнтiв 
        // подана окремим файлом. Призначеннятакож подано кiлькома окремими файлами. Отримати:
        // 1. для кожного пацiєнта ( за iменем i прiзвищем) повний перелiк (безповторень) отриманих 
        // лiкiв iз вказанням сумарної кiлькостi кожного пре-парату в мг;
        // 2. для кожного дня перелiк розходу виданих препаратiв (у мг), впоряд-кований за назвою;
        // 3. для кожного препарату загальний розхiд (у мг) за усi днi.
        
        public static void read_from_txt<T>(List<T> container, string fileName) 
            where T : new() 
        {
            using(var reader = new StreamReader(fileName))
            {
                var atters = reader.ReadLine().Split(',');
                while (!reader.EndOfStream)
                {
                    var values = reader.ReadLine().Split(',');
                    var asc = new T();
                    for (int i = 0; i < atters.Length; i++)
                    {
                        PropertyInfo propertyInfo = asc.GetType().GetProperty(atters[i]);
                        propertyInfo.SetValue(asc, Convert.ChangeType(values[i], propertyInfo.PropertyType), null);
                    }
                    container.Add(asc);
                }
            }
        }

        // 1. для кожного пацiєнта ( за iменем i прiзвищем) повний перелiк (безповторень) отриманих 
        // лiкiв iз вказанням сумарної кiлькостi кожного пре-парату в мг;
        public static void Task1(List<Patient> patients, List<Assignment> assigments, StreamWriter writer)
        {
            var joined = from patient in patients
                join assigment in assigments
                    on patient.Id equals assigment.UserId into j
                from jj in j
                group jj by new {Patient = patient.FullName, Medicune = jj.MedicineName}
                into groupedRes
                select new { Patient = groupedRes.Key.Patient, Medicine = groupedRes.Key.Medicune, 
                    Amount = groupedRes.Sum(el => el.Amount * el.Dose)};

            var result = from gr in joined
                group gr by gr.Patient
                into groupedRes
                select groupedRes;

            foreach (var v in result)
            {
                writer.WriteLine(v.Key);
                foreach (var vv in v)
                    writer.WriteLine($"\t{vv.Medicine}: {vv.Amount}");
            }
        }
        
        // 2. для кожного дня перелiк розходувиданих препаратiв (у мг), впоряд-кований за назвою;
        public static void Task2(List<Patient> patients, List<Assignment> assigments, StreamWriter writer)
        {

            var grouped = from assigment in assigments
                group assigment by new {Date = assigment.Date, Medicune = assigment.MedicineName}
                into groupedRes
                from assignment in groupedRes
                select new
                {
                    Date = groupedRes.Key.Date, Medicine = groupedRes.Key.Medicune,
                    Amount = groupedRes.Sum(el => el.Amount * el.Dose)
                }
                into g
                group g by g.Date
                into result
                select result;
            
            foreach (var v in grouped)
            {
                writer.WriteLine(v.Key);
                foreach (var vv in v)
                    writer.WriteLine($"\t{vv.Medicine}: {vv.Amount}");
            }
        }

        // 3. для кожного препарату загальний розхiд (у мг) за усi днi.
        public static void Task3(List<Patient> patients, List<Assignment> assigments, StreamWriter writer)
        {
            var grouped = from assigment in assigments
                group assigment by assigment.MedicineName
                into groupedRes
                select  groupedRes;
            
            foreach (var v in grouped)
            {
                writer.WriteLine(v.Key);
                writer.WriteLine($"\tSum: {v.Sum(el => el.Amount * el.Dose)}");
            }
            
        }

        public static void Main(string[] args)
        {
            using (StreamWriter writer = new StreamWriter("/Users/sophiyca/RiderProjects/LINQ_3/LINQ_3/results.txt"))
            {
                var patients = new List<Patient>();
                read_from_txt<Patient>(patients,
                    "/Users/sophiyca/RiderProjects/LINQ_3/LINQ_3/patients.csv");
                var assigments = new List<Assignment>();
                read_from_txt<Assignment>(assigments,
                    "/Users/sophiyca/RiderProjects/LINQ_3/LINQ_3/assignments.csv");
                read_from_txt<Assignment>(assigments,
                    "/Users/sophiyca/RiderProjects/LINQ_3/LINQ_3/assignments2.csv");

                
                writer.WriteLine("ALL patients: ");
                foreach (var p in patients)
                    writer.WriteLine($"{p}");
                writer.WriteLine("ALL assigments: ");
                foreach (var p in assigments)
                    writer.WriteLine($"{p}");
                
                writer.WriteLine("\n----------TASK1----------");
                Task1(patients, assigments, writer);
                writer.WriteLine("\n----------TASK2----------");
                Task2(patients, assigments, writer);
                writer.WriteLine("\n----------TASK3----------");
                Task3(patients, assigments, writer);
            }
        }
    }
}