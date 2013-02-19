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
	public class OutCell {
		/// <summary>
		/// 	—сылка на Id €чейки в Ѕƒ
		/// </summary>
		[SerializeNotNullOnly] public int c;

		/// <summary>
		/// 	ѕризнак значени€, которое может быть целью сохранени€
		/// </summary>
		[IgnoreSerialize] public bool canbefilled;

		/// <summary>
		/// 	”никальный »ƒ €чейки
		/// </summary>
		public string i;

		/// <summary>
		/// 	ѕозвол€ет св€зать две €чейки в разных наборах
		/// </summary>
		[IgnoreSerialize] public OutCell linkedcell;

		/// <summary>
		/// 	—сылка на запрос дл€ заполн€емых значений
		/// </summary>
		[IgnoreSerialize] public Query query;

		/// <summary>
		/// 	«начение €чейки
		/// </summary>
		[SerializeNotNullOnly] public string v;
	}
}