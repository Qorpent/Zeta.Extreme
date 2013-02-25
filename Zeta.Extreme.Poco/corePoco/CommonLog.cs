using System;
using Comdiv.Zeta.Model;

using Comdiv.Zeta.Model;namespace Zeta.Extreme.Poco {
    public class CommonLog : ICommonLog{
        #region ICommonLog Members

        public virtual int Id { get; set; }
        public virtual string Code { get; set; }
        public virtual string Type { get; set; }
        public virtual string Usr { get; set; }
        public virtual DateTime Version { get; set; }
        public virtual string Message { get; set; }

        #endregion
    }
}