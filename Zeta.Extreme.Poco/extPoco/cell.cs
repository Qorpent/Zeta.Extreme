#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : cell.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Comdiv.Model;
using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme.Poco {
	public partial class cell : IZetaCell {
#if ADAPTMODEL
		public virtual IZetaRow Meta {
			get { return Row; }
			set { Row = value; }
		}

		public virtual int Month { get; set; }

		private FixRuleResult _fixed = FixRuleResult.Open;

		public virtual FixRuleResult Fixed {
			get { return _fixed; }
			set { _fixed = value; }
		}

		public virtual int FixRuleId { get; set; }


#endif


		public override string ToString() {
			return string.Format("cell:{0}/{1}/{2}/{3}/{4}/{5}/{6}/{7}",
			                     Row.Code, Column.Code, Object.Code, null==DetailObject?"":DetailObject.Code, Year, Period,
			                     DirectDate.ToString("yyyy-MM-dd"), Value
				);
		}
	}
}