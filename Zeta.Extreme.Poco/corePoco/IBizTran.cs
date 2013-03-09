#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IBizTran.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Comdiv.Model;
using Comdiv.Model.Interfaces;
using IEntityDataPattern = Qorpent.Model.IEntity;

namespace Zeta.Extreme.Poco {
	public interface IBizTran : IEntityDataPattern {
		[Deprecated.Map] string FirstType { get; set; }

		[Deprecated.Map] string FirstRole { get; set; }

		[Deprecated.Map] string FirstForm { get; set; }

		[Deprecated.Map] string SecondType { get; set; }

		[Deprecated.Map] string SecondRole { get; set; }

		[Deprecated.Map] string SecondForm { get; set; }
	}
}