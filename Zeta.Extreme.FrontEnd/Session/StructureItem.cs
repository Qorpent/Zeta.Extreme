#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : StructureItem.cs
// Project: Zeta.Extreme.FrontEnd
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Serialization;

namespace Zeta.Extreme.FrontEnd.Session {
	/// <summary>
	/// 	Описывает элемент структуры
	/// </summary>
	[Serialize]
	public class StructureItem {
		/// <summary>
		/// 	Код строки/колонки
		/// </summary>
		public string code;

		/// <summary>
		/// 	Индекс в таблице
		/// </summary>
		public int idx;

		/// <summary>
		/// 	Признак шапки
		/// </summary>
		[SerializeNotNullOnly] public bool iscaption;

		/// <summary>
		/// 	Признак первичности
		/// </summary>
		[SerializeNotNullOnly] public bool isprimary;

		/// <summary>
		/// 	Уровень
		/// </summary>
		[SerializeNotNullOnly] public int level;

		/// <summary>
		/// 	Название строки/колонки
		/// </summary>
		public string name;

		/// <summary>
		/// 	r or c
		/// </summary>
		public string type;
		/// <summary>
		/// Год для колонок
		/// </summary>
		[SerializeNotNullOnly]
		public int year;
		/// <summary>
		/// Период для колонок
		/// </summary>
		[SerializeNotNullOnly]
		public int period;
		/// <summary>
		/// Номер строки
		/// </summary>
		[SerializeNotNullOnly]
		public string number;

		/// <summary>
		///Единица измерения
		/// </summary>
		[SerializeNotNullOnly]
		public string measure;
	}
}