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
// PROJECT ORIGIN: Zeta.Extreme.BizProcess/Attachment.cs
#endregion
using System;
using System.Collections.Generic;
using Qorpent.Serialization;

namespace Zeta.Extreme.BizProcess.Forms {
	/// <summary>
	/// Базовый, универсальный атачмент
	/// </summary>
	[Serialize]
	public class Attachment {
		/// <summary>
		/// 
		/// </summary>
		public Attachment() {
			Metadata = new Dictionary<string, object>();
		}
		/// <summary>
		/// Уникальный глобальный код файла
		/// </summary>
		public string Uid { get; set; }

		/// <summary>
		/// Название
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Комментарии
		/// </summary>
		[SerializeNotNullOnly]
		public string Comment { get; set; }

		/// <summary>
		/// Тип
		/// </summary>
		[SerializeNotNullOnly]
		public string Type { get; set; }

		/// <summary>
		/// Автор аттачмента
		/// </summary>
		public string User { get; set; }

		/// <summary>
		/// Версия файла
		/// </summary>
		public DateTime Version { get; set; }

		/// <summary>
		/// Майм-тип
		/// </summary>
		public string MimeType { get; set; }

		/// <summary>
		/// Хэш файла
		/// </summary>
		[SerializeNotNullOnly]
		public string Hash { get; set; }

		/// <summary>
		/// Размер файла
		/// </summary>
		public long Size { get; set; }

		/// <summary>
		/// Ревизия
		/// </summary>
		public int Revision { get; set; }

		/// <summary>
		/// Дополнительные параметры
		/// </summary>
		[SerializeNotNullOnly]
		public IDictionary<string, object> Metadata { get; private set; }
		/// <summary>
		/// Расширение файла
		/// </summary>
		public string Extension { get; set; }
	}
}