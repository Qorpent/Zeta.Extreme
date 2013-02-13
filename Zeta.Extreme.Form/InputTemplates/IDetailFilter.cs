#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : IDetailFilter.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;
using Comdiv.Zeta.Model;

namespace Zeta.Extreme.Form.InputTemplates {
	/// <summary>
	/// 	Интерфейс фильтров деталей
	/// </summary>
	public interface IDetailFilter {
		/// <summary>
		/// 	Конфигурировать целевую форму
		/// </summary>
		/// <param name="template"> </param>
		void Configure(IInputTemplate template);

		/// <summary>
		/// 	Получить отфильтрованные детали
		/// </summary>
		/// <param name="allObjects"> </param>
		/// <returns> </returns>
		IList<IZetaDetailObject> GetDetails(IEnumerable<IZetaDetailObject> allObjects);
	}
}