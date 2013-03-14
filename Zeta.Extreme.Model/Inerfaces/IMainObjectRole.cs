#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IMainObjectRole.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Model;
using Zeta.Extreme.Model.Deprecated;

namespace Zeta.Extreme.Model.Inerfaces {
	[ForSearch("����, ���, ������� �������")]
	public interface IMainObjectRole :
		IZetaObject,
		IWithMainObjects, IEntity, IWithId, IWithCode, IWithName, IWithTag {
		bool ShowOnStartPage { get; set; }
		}
}