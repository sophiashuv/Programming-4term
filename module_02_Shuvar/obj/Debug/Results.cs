using System.Collections.Generic;
using System.Text.Json;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace module_02_Shuvar
{
    // Результат на етапi 
    // характеризується номером бiгуна, часом промiжно-го фiнiшу на етапi ( у секундах вiд початку змагання),
    // номером етапу i йогопротяжнiстю (в км). Вiдомо, що не всi добiгли до фiнiшу.
    public class Results
    {
        public uint RunnerId { get; set; }
        public List<EtapResults> RunnerResults { get; set; }
        public EtapResults this[int index]
        {
            get
            {
                if (index >= RunnerResults.Count) return new EtapResults(); 
                return RunnerResults[index];
            }
            set => RunnerResults[index] = value;
        }
        
        public Results() => RunnerResults = new List<EtapResults>();

        public Results(uint runnerId, List<EtapResults> res)
        {
            RunnerId = runnerId;
            RunnerResults = new List<EtapResults>();
            foreach (var r in res)
                RunnerResults.Add((EtapResults)r.Clone());
        }
        
        public override string ToString() => JsonSerializer.Serialize(this, new JsonSerializerOptions {WriteIndented = true});
        
    }
}