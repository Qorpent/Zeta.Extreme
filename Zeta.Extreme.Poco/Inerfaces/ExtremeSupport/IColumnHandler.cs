#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IColumnHandler.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme.Poco.Inerfaces {
	/// <summary>
	/// 	Стандартный описатель измерения "колонка"
	/// </summary>
	public interface IColumnHandler : IQueryDimension<IZetaColumn> {
		/// <summary>
		/// 	Простая копия условия на время
		/// </summary>
		/// <returns> </returns>
		IColumnHandler Copy();
	}
}