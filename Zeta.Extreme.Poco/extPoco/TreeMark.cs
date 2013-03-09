#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : TreeMark.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Comdiv.Model.Interfaces;
using Comdiv.Olap.Model;
using Comdiv.Zeta.Model;

namespace Zeta.Extreme.Poco {
	public partial class TreeMark : IZetaRowMark {
		public virtual IEntityDataPattern MarkLinkTarget {
			get { return Target; }
			set { Target = (IZetaRow) value; }
		}

		public virtual IMark MarkLinkMark {
			get { return Mark; }
			set { Mark = value; }
		}
	}
}