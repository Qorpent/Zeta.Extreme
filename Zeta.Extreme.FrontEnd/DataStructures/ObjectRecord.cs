#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : ObjectRecord.cs
// Project: Zeta.Extreme.FrontEnd
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Serialization;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// 	Запись о старшем объекте
	/// </summary>
	[Serialize]
	public class ObjectRecord {
		/// <summary>
		/// 	Дивизион
		/// </summary>
		public string div;

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
		/// 	Короткое имя
		/// </summary>
		public string shortname;
	}
}