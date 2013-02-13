#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : AutoFillType.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme.Form.SaveSupport {
	/// <summary>
	/// 	Тип автозаполнения
	/// </summary>
	public enum AutoFillType {
		/// <summary>
		/// 	Нет
		/// </summary>
		None = 0,

		/// <summary>
		/// 	SQL
		/// </summary>
		Sql = 1,

		/// <summary>
		/// 	Нетиповой
		/// </summary>
		Custom = 2
	}
}