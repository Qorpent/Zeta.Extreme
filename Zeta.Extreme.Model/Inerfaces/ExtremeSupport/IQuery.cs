#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IQuery.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme.Poco.Inerfaces {
	/// <summary>
	/// 	Интерфейс описателя запроса
	/// </summary>
	public interface IQuery : IWithCacheKey {
		/// <summary>
		/// 	Условие на время
		/// </summary>
		ITimeHandler Time { get; set; }

		/// <summary>
		/// 	Условие на строку
		/// </summary>
		IRowHandler Row { get; set; }

		/// <summary>
		/// 	Условие на колонку
		/// </summary>
		IColumnHandler Col { get; set; }

		/// <summary>
		/// 	Условие на объект
		/// </summary>
		IObjHandler Obj { get; set; }

		/// <summary>
		/// 	Выходная валюта
		/// </summary>
		string Valuta { get; set; }

		/// <summary>
		/// 	Простая копия условия на время
		/// </summary>
		/// <param name="deep"> Если да, то делает копии вложенных измерений </param>
		/// <returns> </returns>
		IQuery Copy(bool deep = false);
	}
}