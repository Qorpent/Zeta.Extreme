#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : IStateCheckInterceptor.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Comdiv.Zeta.Model;
using Zeta.Extreme.Form.InputTemplates;

namespace Zeta.Extreme.Form.StateManagement {
	/// <summary>
	/// 	Интерфейс перехватчика проверки статуса
	/// </summary>
	public interface IStateCheckInterceptor {
		/// <summary>
		/// 	Проверка статуса
		/// </summary>
		/// <param name="template"> </param>
		/// <param name="obj"> </param>
		/// <param name="detail"> </param>
		/// <returns> </returns>
		string GetState(IInputTemplate template, IZetaMainObject obj, IZetaDetailObject detail);
	}
}