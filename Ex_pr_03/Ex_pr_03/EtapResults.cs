using System.ComponentModel;

namespace Ex_pr_03
{
    public enum Etap 
    {
        E1 = 100, E2 = 200, E3 = 300
    }
    
    // Результат на етапi характеризується номером бiгуна, часом у секундах, заякий спортсмен подолав 
    // етап, номером етапу i його протяжнiстю (в км)
    public class EtapResults
    {
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
    }
}