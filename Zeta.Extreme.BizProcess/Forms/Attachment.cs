using System;
using System.Collections.Generic;

namespace Zeta.Extreme.BizProcess.Forms {
	/// <summary>
	/// Базовый, универсальный атачмент
	/// </summary>
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
		public string Comment { get; set; }

		/// <summary>
		/// Тип
		/// </summary>
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
		public string Md5 { get; set; }

		/// <summary>
		/// Размер файла
		/// </summary>
		public int Size { get; set; }

		/// <summary>
		/// Ревизия
		/// </summary>
		public int Revision { get; set; }

		/// <summary>
		/// Дополнительные параметры
		/// </summary>
		public IDictionary<string, object> Metadata { get; private set; }
	}
}