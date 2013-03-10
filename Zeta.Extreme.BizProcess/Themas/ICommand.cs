#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : ICommand.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Model;


namespace Zeta.Extreme.BizProcess.Themas {
	/// <summary>
	/// 	Интерфейс команды темы
	/// </summary>
	public interface ICommand : IWithCode, IWithName, IWithRole {
		/// <summary>
		/// 	URL, описывающий некий вызов
		/// </summary>
		string Url { get; set; }
	}
}