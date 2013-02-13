#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : IAutoFillExecutor.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Comdiv.Zeta.Model;

namespace Zeta.Extreme.Form.SaveSupport {
	/// <summary>
	/// 	Интерфейс выпонителя автозаполнения
	/// </summary>
	public interface IAutoFillExecutor {
		/// <summary>
		/// 	Выполнить автозаполнение
		/// </summary>
		/// <param name="autoFill"> </param>
		/// <param name="obj"> </param>
		void Execute(AutoFill autoFill, IZetaMainObject obj);
	}
}