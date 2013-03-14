#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IZoneElement.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Model;

namespace Zeta.Extreme.Model.Inerfaces {
	/// <summary>
	/// �������� ��������� ��������, �������� ��������� � �������� ������� Zeta
	/// </summary>
	public interface IZetaObject : IWithId, IWithCode, IWithName, IWithTag {}
}