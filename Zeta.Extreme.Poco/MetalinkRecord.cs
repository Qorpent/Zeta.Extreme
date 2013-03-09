using System;
using Comdiv.Persistence;

namespace Zeta.Extreme.Poco {
    public class MetalinkRecord {
        public MetalinkRecord() {
            Active = true;
            Start = Qorpent.QorpentConst.Date.Begin;
            Finish = Qorpent.QorpentConst.Date.End;
        }
        public int Id { get; set; }
        [Param]
        public string Code { get; set; }
        [Param]
        public string SrcType { get; set; }
        [Param]
        public string TrgType { get; set; }
        [Param]
        public string Src { get; set; }
        [Param]
        public string Trg { get; set; }
        [Param]
        public string Type { get; set; }
        [Param]
        public string SubType { get; set; }
        [Param]
        public string Tag { get; set;}
        [Param]
        public bool Active { get; set; }

        [Param]
        public DateTime Start { get; set; }

        [Param]
        public DateTime Finish { get; set; }

        public bool UseCustomMapping {
            get { return false; }
        }
    }
}