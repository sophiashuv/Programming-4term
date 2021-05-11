using System;
using System.ComponentModel;

namespace LINQ_5
{
      
    public enum Etap 
    {
        E1 = 100, E2 = 200, E3 = 300
    }
    public class EtapResults: ICloneable
    {
        public EtapResults(uint runnerId, Etap etp, uint time, uint length)
        {
            RunnerId = runnerId;
            Etp = etp;
            Time = time;
            Length = length;
        }
        
        public EtapResults()
        {
            Time = 0;
            Length = 0;
        }
        
        public uint RunnerId { get; set; }
        
        public Etap Etp { get; set; }
        
        public uint Time { get; set;}
        
        public uint Length { get; set; }

        public double Speed => (double) Length / Time;
        
        public override string ToString() {
            string res = "";
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this)) 
                res += ($"{prop.Name}: {prop.GetValue(this)}\n");
            return res;
        }
        public virtual object Clone() => this.MemberwiseClone();
    }
}