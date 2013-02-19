#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IFormSessionControlPointSource.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Zeta.Extreme.Form.StateManagement;

namespace Zeta.Extreme.Form {
	/// <summary>
	/// 	��������� ��������� ����������� ����� �� ������ �����
	/// </summary>
	public interface IFormSessionControlPointSource {
		/// <summary>
		/// 	��������� ����������� �����
		/// </summary>
		ControlPointResult[] ControlPoints { get; }
	}
}