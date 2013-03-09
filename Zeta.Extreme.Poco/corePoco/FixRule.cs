#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : FixRule.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Qorpent;
using Zeta.Extreme.Poco.Deprecated;
using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme.Poco {
	public partial class fixrule : IFixRule {
		[Ref(ClassName = typeof (IZetaRow))] public virtual IZetaRow MainDataTree { get; set; }

		public virtual int Kvart { get; set; }

		[Ref(ClassName = typeof (col))] public virtual IZetaColumn ValueType { get; set; }

		[Ref(ClassName = typeof (detail))] public virtual IZetaDetailObject Subpart { get; set; }

		[Map] public virtual Guid Uid { get; set; }
		[Map] public virtual int Id { get; set; }

		[Map] public virtual DateTime Version { get; set; }

		public virtual IZetaRow Row {
			get { return MainDataTree; }
			set { MainDataTree = value; }
		}

		[Map] public virtual int Year { get; set; }

		[Map] public virtual int Period {
			get { return Kvart; }
			set { Kvart = value; }
		}

		[Map] public virtual DateTime DirectDate {
			get { return ___DirectDate; }
			set { ___DirectDate = value; }
		}

		public virtual IZetaColumn Column {
			get { return ValueType; }
			set { ValueType = value; }
		}


		public virtual IZetaMainObject Object { get; set; }

		public virtual IZetaDetailObject DetailObject {
			get { return Subpart; }
			set { Subpart = value; }
		}

		[Map] public virtual string Comment { get; set; }

		[Map] public virtual string Category { get; set; }

		[Map] public virtual bool Active { get; set; }

		[Map(CustomType = typeof (int))] public virtual FixRulePriority Priority { get; set; }

		[Map(CustomType = typeof (int))] public virtual FixRuleResult Result { get; set; }

		[Map(Formula = "usm.GetFixRuleSalience(Id)")] public virtual int AdvancedWeight { get; set; }
		private DateTime ___DirectDate = QorpentConst.Date.Begin;
	}
}