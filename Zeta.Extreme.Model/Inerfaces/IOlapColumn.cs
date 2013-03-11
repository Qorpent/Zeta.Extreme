#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IOlapColumn.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Model;

namespace Zeta.Extreme.Poco.Inerfaces {
	public interface IOlapColumn :
		IDimension,
		IEntity,
		IOlapEvaluationExtensions,
		IWithDataType {}
}