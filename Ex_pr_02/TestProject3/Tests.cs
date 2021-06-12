using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Ex_pr_02;
using Xunit;

namespace TestProject3
{
    public class Tests
    {
        private List<Visitors> TestVisitors = new List<Visitors>()
        {
            new Visitors()
            {
                Id = 1,
                Surname = "Surname1",
                Name = "Visitor1",
                Discount = 20
            },
            new Visitors()
            {
                Id = 2,
                Surname = "Surname2",
                Name = "Visitor2",
                Discount = 30
            }
        };
        
        private List<Service> TestServices = new List<Service>()
        {
            new Service()
            {
                Name = "Service1",
                Id = 1,
                Price = 34
            }
        };
        private List<Trainings> TestTrainings = new List<Trainings>()
        {
            new Trainings()
            {
                Date = DateTime.Parse("2021-02-24"),
                VisitorId = 1,
                ServiceId = 1,
                Duration = 3
            },
            new Trainings()
            {
                Date = DateTime.Parse("2020-02-24"),
                VisitorId = 2,
                ServiceId = 1,
                Duration = 5
            }
        };

        public List<Visitors> TestVisitors1 => TestVisitors;

        public List<Service> TestServices1 => TestServices;

        public List<Trainings> TestTrainings1 => TestTrainings;

        [Fact]
        public void TestTask1()
        {
            var xElement = Program.Task1(TestServices1, TestVisitors1, TestTrainings1);
            
            var expected = new XElement("VisitorsPrices",
                new XElement("Info",
                    new XElement("Visitor", "Visitor1 Surname1"),
                    new XElement("Price", 20.4)),
                new XElement("Info",
                    new XElement("Visitor", "Visitor2 Surname2"),
                    new XElement("Price", 51))
            );
            
            Assert.Equal(xElement.ToString(), expected.ToString());

        }
        
        [Fact]
        public void TestTask2()
        {
            var xElement = Program.Task2(TestServices1, TestVisitors1, TestTrainings1);
            
            var expected = new XElement("Task2",
                new XElement("Service",
                    new XAttribute("Name", "Service1"),
                    new XElement("Date", 
                        new XAttribute("Day", "2021-24-02"), 
                        new XElement("Duration", 3),
                        new XElement("Price", 20.4)),
                    new XElement("Date", 
                        new XAttribute("Day", "2020-24-02"), 
                        new XElement("Duration", 5),
                        new XElement("Price", 51)))
                );
            
            Assert.Equal(xElement.ToString(), expected.ToString());
        }
        
        [Fact]
        public void TestTask3()
        {
            var xElement = Program.Task3(TestServices1, TestVisitors1, TestTrainings1);

            var expected = new XElement("Task3",
                new XElement("Service",
                    new XAttribute("Name", "Service1"),
                    new XElement("Date", 
                        new XAttribute("Day", "2020-24-02"),
                        new XElement("Price", 51)),
                    new XElement("Date", 
                        new XAttribute("Day", "2021-24-02"),
                        new XElement("Price", 20.4)))
            );
            
            Assert.Equal(xElement.ToString(), expected.ToString());
        }
    }
}