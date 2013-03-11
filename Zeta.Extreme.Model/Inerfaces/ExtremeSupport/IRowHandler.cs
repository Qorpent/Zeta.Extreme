#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IRowHandler.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Poco.Inerfaces {
	/// <summary>
	/// 	—тандартный описатель измерени€ Row
	/// </summary>
	public interface IRowHandler : IQueryDimension<IZetaRow> {
		/// <summary>
		/// 	True если целева€ строка - ссылка
		/// </summary>
		bool IsRef { get; }

		/// <summary>
		/// 	True если целева€ строка - ссылка
		/// </summary>
		bool IsSum { get; }

		/// <summary>
		/// 	ѕроста€ копи€ услови€ на строку
		/// </summary>
		/// <returns> </returns>
		IRowHandler Copy();

		/// <summary>
		/// 	Ќормализует ссылки и параметры
		/// </summary>
		/// <param name="session"> </param>
		/// <param name="column"> </param>
		void Normalize(ISession session, IZetaColumn column);
	}
}