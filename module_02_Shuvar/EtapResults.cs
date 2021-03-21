using System;
using System.ComponentModel;

namespace module_02_Shuvar
{
    public enum Etap 
    {
        E1 = 100, E2 = 200, E3 = 300
    }
    public class EtapResults: ICloneable
    {
        public EtapResults(Etap etp, uint time, uint length)
        {
            Etp = etp;
            Time = time;
            Length = length;
        }
        
        public EtapResults()
        {
            Time = 0;
            Length = 0;
        }
        
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