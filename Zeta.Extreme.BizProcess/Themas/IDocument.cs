#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IDocument.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Model;

namespace Zeta.Extreme.BizProcess.Themas {
	/// <summary>
	/// 	Описатель документа темы
	/// </summary>
	public interface IDocument : IWithCode, IWithName, IWithRole {
		/// <summary>
		/// 	Тип документа
		/// </summary>
		string Type { get; set; }

		/// <summary>
		/// 	Ссылка на документ
		/// </summary>
		string Url { get; set; }

		/// <summary>
		/// 	Значение
		/// </summary>
		string Value { get; set; }
	}
}