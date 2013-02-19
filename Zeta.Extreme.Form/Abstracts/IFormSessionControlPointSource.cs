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
	/// 	Интерфейс получения контрольных точек из сессии формы
	/// </summary>
	public interface IFormSessionControlPointSource {
		/// <summary>
		/// 	Коллекция контрольных точек
		/// </summary>
		ControlPointResult[] ControlPoints { get; }
	}
}