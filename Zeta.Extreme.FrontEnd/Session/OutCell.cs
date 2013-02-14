#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : OutCell.cs
// Project: Zeta.Extreme.FrontEnd
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Serialization;

namespace Zeta.Extreme.FrontEnd.Session {
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
		/// 	”никальный »ƒ €чейки
		/// </summary>
		public string i;

		/// <summary>
		/// 	«начение €чейки
		/// </summary>
		[SerializeNotNullOnly] public string v;
	}
}