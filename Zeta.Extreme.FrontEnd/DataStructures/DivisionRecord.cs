#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : DivisionRecord.cs
// Project: Zeta.Extreme.FrontEnd
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Serialization;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// 	Запись о дивизионе
	/// </summary>
	[Serialize]
	public class DivisionRecord {
		/// <summary>
		/// 	код дивизиона
		/// </summary>
		public string code;

		/// <summary>
		/// 	Индекс дивизиона
		/// </summary>
		public int idx;

		/// <summary>
		/// 	Название дивизиона
		/// </summary>
		public string name;
	}
}