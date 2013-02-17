#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IThemaFactoryProvider.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme.Form.Themas {
	/// <summary>
	/// 	Простой интерфейс поставщика фабрики тем
	/// </summary>
	public interface IThemaFactoryProvider {
		/// <summary>
		/// 	Получить фабрику
		/// </summary>
		/// <returns> </returns>
		IThemaFactory Get();

		/// <summary>
		/// 	Перегрузить фабрику
		/// </summary>
		void Reload();
	}
}