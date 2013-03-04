#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IThemaConfigurationProvider.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme.Form.Themas {
	/// <summary>
	/// 	Фабричный интерфейс для конфигураций темы
	/// </summary>
	public interface IThemaConfigurationProvider {
		/// <summary>
		/// 	Опции зашгрузки темы
		/// </summary>
		ThemaLoaderOptions Options { get; set; }

		/// <summary>
		/// 	Получить конфигурацию
		/// </summary>
		/// <returns> </returns>
		IThemaFactoryConfiguration Get();

		/// <summary>
		/// 	Установить значение параметра для конкретной темы
		/// </summary>
		/// <param name="themacode"> </param>
		/// <param name="parameter"> </param>
		/// <param name="value"> </param>
		void Set(string themacode, string parameter, object value);
	}
}