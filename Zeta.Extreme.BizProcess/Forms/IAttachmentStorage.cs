using System.Collections.Generic;
using System.IO;

namespace Zeta.Extreme.BizProcess.Forms {
	/// <summary>
	/// Хранилище присоединенного контента
	/// </summary>
	public interface IAttachmentStorage {
		/// <summary>
		/// Осуществляет поиск аттачментов с указанной маской поиска
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		IEnumerable<Attachment> Find(Attachment query);
		/// <summary>
		/// Сохраняет аттачмент в хранилище
		/// </summary>
		/// <param name="attachment"></param>
		void Save(Attachment attachment);

		/// <summary>
		/// Открывает поток на запись контента
		/// </summary>
		/// <param name="attachment"></param>
		/// <returns></returns>
		Stream OpenWrite(Attachment attachment);
		/// <summary>
		/// Открывает поток на чтение контента
		/// </summary>
		/// <param name="attachment"></param>
		/// <returns></returns>
		Stream OpenRead(Attachment attachment);
	}
}