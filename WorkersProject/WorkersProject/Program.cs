using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace WorkersProject
{
    internal class Program
    {
        public interface IReportable
        {
            double Taxes();
        }

        public class Worker: IReportable, ICloneable
        {
            public static double p3 = 5;
            
            public Worker(string name, string surname, double salary)
            {
                Name = name;
                Surname = surname;
                Salary = salary;
            }
            public Worker()
            {
            }
            public string Name { get; set; }
            public string Surname { get; set; }
            public double Salary { get; set; }

            public string FullName => Name + " " + Surname;

            public double Taxes() => Salary >= 10000 ? Salary * p3 / 100 : 0;
            
            public virtual object Clone() => this.MemberwiseClone();
            
            public override string ToString() {
                string res = $"{this.GetType().Name}:\n";
                foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this)) 
                    res += ($"\t{prop.Name}: {prop.GetValue(this)}\n");
                return res;
            }
        }
        
        public class FOP: Worker
        {
            public static double p3 = 10;
            public double Taxes() => Salary * p3 / 100;
        }

        public class Company: IReportable, IEnumerable<Worker>
        {
            public static double p1 = 20;
            public static double p2 = 10;
            
            public Company(string name, double income, List<Worker> workers)
            {
                Name = name;
                Income = income;
                foreach (var worker in workers)
                    Workers.Add((Worker) worker.Clone());
            }
            
            public Company()
            {
            }
            public string Name { get; set; }
            public double Income { get; set; }
            public List<Worker> Workers { get; set;}
            
            public Worker this[int index]
            {
                get => Workers[index];
                set => Workers[index] = value;
            }
            
            public  IEnumerator<Worker> GetEnumerator() => Workers.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
            
            public double Taxes() => TaxCompanyWorker() + TaxCompany();
            private double TaxCompanyWorker() => Workers.Sum(worker=>worker.Salary * (p2/100));
            
            private double TaxCompany() => Income * (p1 / 100);

            public override string ToString()
            {
                string res = $"{Name} : {Income}\n";
                foreach (var worker in Workers)
                {
                    res += worker.ToString() + "\n";
                }
                return res;
            }
        }
        
        class WorkerComparer : IComparer<Worker>
        {
            public int Compare(Worker x, Worker y)
            {
                return x.Surname.CompareTo(y.Surname);
            }
        }

        public static void Main(string[] args)
        {
            FOP w1 = new FOP { Name = "Tom", Surname = "Ward", Salary = 9000};
            Worker w2 = new Worker { Name = "John", Surname = "Black", Salary = 19000};
            Worker w3 = new Worker { Name = "Jack", Surname = "Firget", Salary = 10000};
            Worker w4 = new Worker { Name = "Molli", Surname = "Pifar", Salary = 5000};

            Company c1 = new Company { Name = "Company1", Income = 200000, Workers = new List<Worker>(){ w2, w3, w4 } };

            Worker w5 = new Worker { Name = "Mary", Surname = "Mors", Salary = 7000};
            Worker w6 = new Worker { Name = "Sam", Surname = "Watson", Salary = 23000};
            Worker w7 = new Worker { Name = "Dean", Surname = "Rabet", Salary = 12500};
            FOP w8 = new FOP { Name = "Sally", Surname = "Qurf", Salary = 6900};
            Company c2 = new Company { Name = "Company2", Income = 100000, Workers = new List<Worker>() { w5, w6, w7} };

            IReportable[] items = new IReportable[] { w1, w8, c1, c2 };
            var sum = items.Sum(x => x.Taxes());
            Console.WriteLine("Sum of all taxes = " + sum);
            
            var companies = new List<Company> { c1, c2 };
            var maxCompany = companies.OrderByDescending(obj => obj.Taxes()).First();
            Console.WriteLine($"\nCompany with max tax: \n{maxCompany}");
            Console.WriteLine("\nWorkers:");
            
            maxCompany.Workers.Sort(new WorkerComparer());
            Console.WriteLine(maxCompany);
            Console.WriteLine();
        }
    }
}