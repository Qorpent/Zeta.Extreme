#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IStateCheckInterceptor.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Zeta.Extreme.BizProcess.Themas;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.BizProcess.StateManagement {
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