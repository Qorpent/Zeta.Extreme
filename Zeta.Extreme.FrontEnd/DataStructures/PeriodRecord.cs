#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : PeriodRecord.cs
// Project: Zeta.Extreme.FrontEnd
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Serialization;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// 	Запись о периоде
	/// </summary>
	[Serialize]
	public class PeriodRecord {
		/// <summary>
		/// 	Ид периода (ClassicId)
		/// </summary>
		public int id;

		/// <summary>
		/// 	Индекс периода в рамках типа
		/// </summary>
		public int idx;

		/// <summary>
		/// 	Название периода
		/// </summary>
		public string name;

		/// <summary>
		/// 	Тип периода
		/// </summary>
		public PeriodType type;
	}
}