using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Zeta.Extreme.BizProcess.Forms {
	/// <summary>
	///Служба для работы с аттачами формы
	/// </summary>
	public class FormAttachmentSource : IFormAttachmentStorage {
		private IAttachmentStorage _storage;
		/// <summary>
		/// Устанавливает службу хранения
		/// </summary>
		/// <param name="storage"></param>
		public void SetStorage(IAttachmentStorage storage) {
			_storage = storage;
		}

		/// <summary>
		/// Найти все присоедиеннные файлы, реферирующиеся с данной сессией
		/// </summary>
		/// <returns></returns>
		public IEnumerable<FormAttachment> GetAttachments(IFormSession session) {
			var query = new FormAttachment(session, null, AttachedFileType.Default, false);
			return _storage.Find(query).Select(_ => new FormAttachment(session, _, AttachedFileType.Default));
		}

		/// <summary>
		/// Сохраняет аттачмент, связанный с текущей сессией
		/// </summary>
		/// <param name="session">целевая сессия </param>
		/// <param name="attachment">присоединенный контент</param>
		public FormAttachment SaveAttachment(IFormSession session, Attachment attachment) {
			var realattach = new FormAttachment(session, attachment, AttachedFileType.Default, false);
			_storage.Save(realattach);
			return realattach;
		}

		/// <summary>
		/// Открывает поток на запись контента
		/// </summary>
		/// <param name="attachment"></param>
		/// <returns></returns>
		public Stream OpenWrite(FormAttachment attachment) {
			return _storage.OpenWrite(attachment);
		}

		/// <summary>
		/// Открывает поток на чтение контента
		/// </summary>
		/// <param name="attachment"></param>
		/// <returns></returns>
		public Stream OpenRead(FormAttachment attachment) {
			return _storage.OpenRead(attachment);
		}
	}
}