using System;
using System.ComponentModel;

namespace LINQ_3
{
    public class Assignment
    {
        public Assignment()
        {
        }
        
        public Assignment(uint userId, DateTime date, string medicineName, double dose, int amount)
        {
            UserId = userId;
            Date = date;
            MedicineName = medicineName;
            Dose = dose;
            Amount = amount;
        }

        public uint UserId { get; set; }
        public DateTime Date { get; set; }
        public string MedicineName { get; set; }
        public double Dose { get; set; }
        public int Amount { get; set; }
        public virtual double AllAmount => Dose * Amount;

        public override string ToString() {
            string res = "";
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this)) 
                res += ($"\t{prop.Name}: {prop.GetValue(this)}\n");
            return res;
        }
    }
}