#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : OutCell.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Serialization;

namespace Zeta.Extreme.Form {
	/// <summary>
	/// 	ќписывает €чейку данных
	/// </summary>
	[Serialize]
	public class OutCell : OutCellBase {
		/// <summary>
		/// 	—сылка на запрос дл€ заполн€емых значений
		/// </summary>
		[IgnoreSerialize] public Query query;
	}
}