using System;
using System.Collections.Generic;
using Qorpent.Serialization;

namespace Zeta.Extreme.BizProcess.Forms {
	/// <summary>
	/// Элемент записи в бло
	/// </summary>
	[Serialize]
	public class FormChatItem {
		/// <summary>
		/// 
		/// </summary>
		public FormChatItem() {
			Userdata  = new Dictionary<string, object>();
		}
		/// <summary>
		/// Уникальный идентификатор записи
		/// </summary>
		public string Id { get; set; }
		/// <summary>
		/// Пользователь записи
		/// </summary>
		public string User { get; set; }
		/// <summary>
		/// Время запиcи
		/// </summary>
		public DateTime Time { get; set; }
		/// <summary>
		/// Текст сообщения
		/// </summary>
		public string Text { get; set; }
		/// <summary>
		/// Код формы
		/// </summary>
		public string FormCode { get; set; }
		/// <summary>
		/// Предприятие
		/// </summary>
		public int ObjId { get; set; }
		/// <summary>
		/// Год
		/// </summary>
		public int Year { get; set; }
		/// <summary>
		/// Период
		/// </summary>
		public int Period { get; set; }
		/// <summary>
		/// Тип сообщения
		/// </summary>
		public string Type { get; set; }

		/// <summary>
		/// Дополнительные данные
		/// </summary>
		[Serialize]
		[SerializeNotNullOnly]
		public IDictionary<string,object> Userdata { get; set; }
	}
}