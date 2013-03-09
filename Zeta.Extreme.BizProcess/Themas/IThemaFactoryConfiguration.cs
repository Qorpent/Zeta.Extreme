#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IThemaFactoryConfiguration.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;
using System.Xml.Linq;

namespace Zeta.Extreme.BizProcess.Themas {
	/// <summary>
	/// 	Интерфейс конфигуратора фабрики тем
	/// </summary>
	public interface IThemaFactoryConfiguration {
		/// <summary>
		/// 	Список конфигурации
		/// </summary>
		IList<IThemaConfiguration> Configurations { get; }

		/// <summary>
		/// 	Исходный XML
		/// </summary>
		XElement[] SrcXml { get; set; }

		/// <summary>
		/// 	Вызов метода конфигурации фабрики
		/// </summary>
		/// <returns> </returns>
		IThemaFactory Configure();
	}
}