using System;
using System.ComponentModel;

namespace Ex_pr_02
{
    // < дата, iдентифiкацiйний номер вiдвiдувача, номер сервiсу, тривалiсть в годинах > 
    public class Trainings
    {
        private double _duration;
        
        public DateTime Date { get; set; }
        public uint VisitorId { get; set; }
        public uint ServiceId { get; set; }
        public double Duration{
            get => _duration;
            set => _duration = Validation.ValidatePositive(value); 
        }

        public override string ToString() {
            string res = "";
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this)) 
                res += ($"{prop.Name}: {prop.GetValue(this)}\n");
            return res;
        }
    }
}