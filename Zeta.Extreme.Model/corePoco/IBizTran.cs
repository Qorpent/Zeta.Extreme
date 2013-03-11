#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IBizTran.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Zeta.Extreme.Poco.Deprecated;
using IEntityDataPattern = Qorpent.Model.IEntity;

namespace Zeta.Extreme.Poco {
	public interface IBizTran : IEntityDataPattern {
		[Map] string FirstType { get; set; }

		[Map] string FirstRole { get; set; }

		[Map] string FirstForm { get; set; }

		[Map] string SecondType { get; set; }

		[Map] string SecondRole { get; set; }

		[Map] string SecondForm { get; set; }
	}
}