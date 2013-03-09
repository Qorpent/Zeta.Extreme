#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : SubpartMark.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Comdiv.Model.Interfaces;
using Zeta.Extreme.Poco.Inerfaces;
using IEntityDataPattern = Qorpent.Model.IEntity;

namespace Zeta.Extreme.Poco {
	public partial class SubpartMark : IZetaDetailObjectMark {
		public virtual IEntityDataPattern MarkLinkTarget {
			get { return Target; }
			set { Target = (IZetaDetailObject) value; }
		}

		public virtual IMark MarkLinkMark {
			get { return Mark; }
			set { Mark = value; }
		}
	}
}