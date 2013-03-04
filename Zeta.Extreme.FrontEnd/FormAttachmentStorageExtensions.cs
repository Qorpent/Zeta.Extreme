using System;
using System.Web;
using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// ¬спомогательный класс дл€ работы с хранилищем
	/// </summary>
	public static class FormAttachmentStorageExtensions {
		/// <summary>
		/// ¬спомогательный метод дл€ быстрой прив€зки файла из сессии
		/// </summary>
		/// <param name="storage">целевое хранилище </param>
		/// <param name="session">сесси€</param>
		/// <param name="file">файл, переданный по Http</param>
		/// <param name="doctype">тип аттача (параметр)</param>
		/// <param name="existeduid">переписвание существующего файла</param>
		/// <returns></returns>
		public static FormAttachment AttachHttpFile(this IFormAttachmentStorage storage, IFormSession session, HttpPostedFile file, string doctype ,string existeduid="") {
			throw new NotImplementedException();
		}
	}
}