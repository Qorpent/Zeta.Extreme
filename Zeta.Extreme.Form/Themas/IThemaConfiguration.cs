#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : IThemaConfiguration.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;
using Comdiv.Model.Interfaces;

namespace Zeta.Extreme.Form.Themas {
	/// <summary>
	/// 	Интерфейс конфигурации темы
	/// </summary>
	public interface IThemaConfiguration : IWithCode, IWithName, IWithIdx {
		/// <summary>
		/// 	Признак активности конфигурации
		/// </summary>
		bool Active { get; set; }

		/// <summary>
		/// 	Роль элемента по умолчанию
		/// </summary>
		string DefaultElementRole { get; set; }

		/// <summary>
		/// 	Импортированные конфигурации
		/// </summary>
		IList<IThemaConfiguration> Imports { get; set; }

		/// <summary>
		/// 	Строка, описывающая происхождение конфигурации
		/// </summary>
		string Evidence { get; }

		/// <summary>
		/// 	Метод конструирования темы
		/// </summary>
		/// <returns> </returns>
		IThema Configure();

		/// <summary>
		/// 	Метод разрешения параметра
		/// </summary>
		/// <param name="name"> </param>
		/// <param name="def"> </param>
		/// <returns> </returns>
		TypedParameter ResolveParameter(string name, object def = null);
	}
}