#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : ICommonLog.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Model;

namespace Zeta.Extreme.Poco.Inerfaces {
	public interface ICommonLog : IWithId, IWithVersion, IWithCode, IWithUsr {
		string Type { get; set; }
		string Message { get; set; }
	}
}