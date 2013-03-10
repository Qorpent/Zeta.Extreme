#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : GraphRelation.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Zeta.Extreme.Poco.Deprecated;
using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme.Poco {
	public class GraphRelation : IGraphRelation {
		public virtual string TypeString {
			get { return Type.ToString(); }
			set { Type = (GraphRelationType) Enum.Parse(typeof (GraphRelationType), value, true); }
		}

		public virtual Guid Uid { get; set; }
		public virtual int Id { get; set; }
		public virtual DateTime Version { get; set; }
		public virtual string Code { get; set; }

		[Ref(Alias = "Org", Title = "Корень", Nullable = true, IsAutoComplete = true, AutoCompleteType = "org")] public virtual IZetaMainObject Root { get; set; }

		[Ref(Alias = "Org", Title = "Цель", Nullable = true, IsAutoComplete = true, AutoCompleteType = "org")] public virtual
			IZetaMainObject Target { get; set; }


		public virtual GraphRelationType Type { get; set; }

		[Map] public virtual string TypeDetail { get; set; }
	}
}