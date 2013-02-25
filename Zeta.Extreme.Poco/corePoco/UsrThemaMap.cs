using System;

using Comdiv.Zeta.Model;namespace Zeta.Extreme.Poco {



    public class UsrThemaMap : IUsrThemaMap{
        #region IUsrThemaMap Members

        public virtual int Id { get; set; }
        public virtual IZetaUnderwriter Usr { get; set; }
        public virtual IZetaMainObject Object { get; set; }
        public virtual string System { get; set; }
        public virtual string Thema { get; set; }
        public virtual DateTime Version { get; set; }

    	public virtual bool IsPlan {
			get { return Thema.EndsWith("_2"); }
    	}

    	public virtual string ThemaCode {
			get { return Thema.Replace("_2", ""); }
    	}

        #endregion
    }
}