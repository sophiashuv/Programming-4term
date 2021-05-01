using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LINQ_2
{
    //     Результати iспитiв характеризується на-звою предмета, номером талона (екзкменацiйна 
    //     вiдомiсть – це талон No1),номером студента, кiлькiстю балiв в семестрi i за предмет 
    //     вцiлому. 
    
    public enum Talon {Talon1 = 1, Talon2 = 2, Talon3 = 3, Talon4 = 4}
    
    public class Result
    {
        public string Subject { get; set; }
        
        public Talon TalonNum { get; set; }
        
        public  String StudentId { get; set; }
        
        [RangeAttribute(0, 50)]
        public uint TermPoints { get; set;}
        
        [RangeAttribute(0, 100)]
        public uint TotalPoints { get; set;}
        
        public Result() {}

        public Result(string subject, Talon talonNum, String studentId, uint termPoints, uint totalPoints)
        {
            Subject = subject;
            TalonNum = talonNum;
            StudentId = studentId;
            TermPoints = termPoints;
            TotalPoints = totalPoints;
        }
        
        public override string ToString() {
            string res = "";
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this)) 
                res += ($"\t{prop.Name}: {prop.GetValue(this)}\n");
            return res;
        } 
    }
}