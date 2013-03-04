#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : Document.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Zeta.Extreme.BizProcess.Themas;

namespace Zeta.Extreme.Form.Themas {
	/// <summary>
	/// 	Документ темы
	/// </summary>
	public class Document : IDocument {
		/// <summary>
		/// 	Код
		/// </summary>
		public string Code { get; set; }

		/// <summary>
		/// 	Название
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 	Роль доступа
		/// </summary>
		public string Role { get; set; }

		/// <summary>
		/// 	Тип документа
		/// </summary>
		public string Type { get; set; }

		/// <summary>
		/// 	Ссылка на документ
		/// </summary>
		public string Url { get; set; }

		/// <summary>
		/// 	Значение
		/// </summary>
		public string Value { get; set; }
	}
}