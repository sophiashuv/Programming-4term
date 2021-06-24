using System;
using System.Collections.Generic;

namespace Exam1
{
    public class Results
    {
        public string Subject { get; set; }
        public List<Result> Res{ get; set; }
       
        public override string ToString() {
            string res = Subject + ":\n";
            res += String.Join("\n", Res);
            return res;
        }
    }
}