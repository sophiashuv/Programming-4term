using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace module_06_Shuvar
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
            var totSum = new Dictionary<string, Dictionary<string, double>>();
            foreach (var patient in patients)
            {
                foreach (var assignment in assigments)
                {
                    if (patient.Id == assignment.UserId)
                    {
                        if (totSum.ContainsKey(patient.FullName))
                        {
                            if (totSum[patient.FullName].ContainsKey(assignment.MedicineName))
                                totSum[patient.FullName][assignment.MedicineName] += assignment.AllAmount;
                            else
                            {
                                totSum[patient.FullName].Add(assignment.MedicineName, assignment.AllAmount);
                            } 
                        }
                        else
                        {
                            var d = new Dictionary<string, double>();
                            d.Add(assignment.MedicineName, assignment.AllAmount);
                            totSum.Add(patient.FullName, d);
                        }
                    }
                }
            }

            foreach (var kvp in totSum){
                writer.WriteLine($"{kvp.Key}: ");
                foreach (var kvpp in totSum[kvp.Key])
                    writer.WriteLine($"\t{kvpp.Key} - {kvpp.Value}");
            }
        }
        
        // 2. для кожного дня перелiк розходу виданих препаратiв (у мг), впоряд-кований за назвою;
        public static void Task2(List<Patient> patients, List<Assignment> assigments, StreamWriter writer)
        {
            var totSum = new Dictionary<DateTime, Dictionary<string, double>>();
            foreach (var assignment in assigments) 
            {
                if (totSum.ContainsKey(assignment.Date))
                {
                    if (totSum[assignment.Date].ContainsKey(assignment.MedicineName))
                        totSum[assignment.Date][assignment.MedicineName] += assignment.AllAmount;
                    else
                    {
                        totSum[assignment.Date].Add(assignment.MedicineName, assignment.AllAmount);
                    } 
                }
                else
                {
                    var d = new Dictionary<string, double>();
                    d.Add(assignment.MedicineName, assignment.AllAmount);
                    totSum.Add(assignment.Date, d);
                }
            }

            foreach (var kvp in totSum){
                writer.WriteLine($"{kvp.Key}: ");
                foreach (var kvpp in totSum[kvp.Key])
                    writer.WriteLine($"\t{kvpp.Key} - {kvpp.Value}");
            }
        }

        // 3. для кожного препарату загальний розхiд (у мг) за усi днi.
        public static void Task3(List<Patient> patients, List<Assignment> assigments, StreamWriter writer)
        {
            var totSum = new Dictionary<string, Dictionary<DateTime, double>>();
            foreach (var assignment in assigments)
            {
                if (totSum.ContainsKey(assignment.MedicineName))
                {
                    if (totSum[assignment.MedicineName].ContainsKey(assignment.Date))
                            totSum[assignment.MedicineName][assignment.Date] += assignment.AllAmount;
                    else
                        totSum[assignment.MedicineName].Add(assignment.Date, assignment.AllAmount);
                }
                else
                {
                    var d = new Dictionary<DateTime, double>();
                    d.Add(assignment.Date, assignment.AllAmount);
                    totSum.Add(assignment.MedicineName, d);
                }
            }
            
            foreach (var kvp in totSum)
            {
                writer.WriteLine($"{kvp.Key} - sum:{kvp.Value.Sum(x => x.Value)}");
                foreach (var kvpp in totSum[kvp.Key])
                    writer.WriteLine($"\t{kvpp.Key} - {kvpp.Value}");
            }
        }

        public static void Main(string[] args)
        {
            using (StreamWriter writer = new StreamWriter("/Users/sophiyca/RiderProjects/module_06_Shuvar/module_06_Shuvar/result.txt"))
            {
                var patients = new List<Patient>();
                read_from_txt<Patient>(patients,
                    "/Users/sophiyca/RiderProjects/module_06_Shuvar/module_06_Shuvar/patients.csv");
                var assigments = new List<Assignment>();
                read_from_txt<Assignment>(assigments,
                    "/Users/sophiyca/RiderProjects/module_06_Shuvar/module_06_Shuvar/assigments.csv");
                read_from_txt<Assignment>(assigments,
                    "/Users/sophiyca/RiderProjects/module_06_Shuvar/module_06_Shuvar/assigments2.csv");

                
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