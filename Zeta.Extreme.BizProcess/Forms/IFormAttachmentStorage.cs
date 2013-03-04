using System.Collections.Generic;
using System.IO;
using System.Web;

namespace Zeta.Extreme.BizProcess.Forms {
	/// <summary>
	/// Интерфейс хранилища аттачментов данных форм
	/// </summary>
	public interface IFormAttachmentStorage {
		/// <summary>
		/// Найти все присоедиеннные файлы, реферирующиеся с данной сессией
		/// </summary>
		/// <returns></returns>
		IEnumerable<FormAttachment> GetAttachments(IFormSession session);

		/// <summary>
		/// Сохраняет аттачмент, связанный с текущей сессией
		/// </summary>
		/// <param name="session">целевая сессия </param>
		/// <param name="attachment">присоединенный контент</param>
		FormAttachment SaveAttachment(IFormSession session, Attachment attachment);

		/// <summary>
		/// Открывает поток на запись контента
		/// </summary>
		/// <param name="attachment"></param>
		/// <returns></returns>
		Stream OpenWrite(FormAttachment attachment);
		/// <summary>
		/// Открывает поток на чтение контента
		/// </summary>
		/// <param name="attachment"></param>
		/// <returns></returns>
		Stream OpenRead(FormAttachment attachment);
	}
}