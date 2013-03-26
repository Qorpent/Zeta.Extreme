#region LICENSE
// Copyright 2007-2013 Qorpent Team - http://github.com/Qorpent
// Supported by Media Technology LTD 
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//  
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// PROJECT ORIGIN: Zeta.Extreme.Form/Document.cs
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