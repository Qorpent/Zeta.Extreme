#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IZetaFormsSupport.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using IWithIdx = Qorpent.Model.IWithIdx;

namespace Zeta.Extreme.Poco.Inerfaces {
	public interface IZetaFormsSupport :
		IWithIdx,
		IWithOuterCode,
		IWithGroup {}
}