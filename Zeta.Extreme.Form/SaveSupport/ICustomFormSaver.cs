#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : ICustomFormSaver.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Zeta.Extreme.Form.InputTemplates;

namespace Zeta.Extreme.Form.SaveSupport {
	/// <summary>
	/// 	Нестандартный класс сохранения данных формы
	/// </summary>
	public interface ICustomFormSaver {
		/// <summary>
		/// 	Сохраняет данные формы
		/// </summary>
		/// <param name="request"> </param>
		/// <param name="xml"> </param>
		/// <returns> </returns>
		string Save(InputTemplateRequest request, string xml);
	}
}