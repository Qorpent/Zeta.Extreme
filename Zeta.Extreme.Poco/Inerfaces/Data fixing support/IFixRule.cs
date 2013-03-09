#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IFixRule.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Comdiv.Model;
using Comdiv.Model.Interfaces;
using Comdiv.Olap.Model;

namespace Comdiv.Zeta.Model {
	public interface IFixRule :
		IItemDataPattern,
		IOlapVector<IZetaRow, IZetaColumn, IZetaMainObject, IZetaDetailObject>,
		IWithComment,
		IWithCategory,
		IWithActive,
		IFixRuleCore {}
}