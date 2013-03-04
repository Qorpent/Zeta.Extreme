using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Zeta.Extreme.BizProcess.Forms {
	/// <summary>
	///������ ��� ������ � �������� �����
	/// </summary>
	public class FormAttachmentSource : IFormAttachmentStorage {
		private IAttachmentStorage _storage;
		/// <summary>
		/// ������������� ������ ��������
		/// </summary>
		/// <param name="storage"></param>
		public void SetStorage(IAttachmentStorage storage) {
			_storage = storage;
		}

		/// <summary>
		/// ����� ��� �������������� �����, �������������� � ������ �������
		/// </summary>
		/// <returns></returns>
		public IEnumerable<FormAttachment> GetAttachments(IFormSession session) {
			var query = new FormAttachment(session, null, AttachedFileType.Default, false);
			return _storage.Find(query).Select(_ => new FormAttachment(session, _, AttachedFileType.Default));
		}

		/// <summary>
		/// ��������� ���������, ��������� � ������� �������
		/// </summary>
		/// <param name="session">������� ������ </param>
		/// <param name="attachment">�������������� �������</param>
		public FormAttachment SaveAttachment(IFormSession session, Attachment attachment) {
			var realattach = new FormAttachment(session, attachment, AttachedFileType.Default, false);
			_storage.Save(realattach);
			return realattach;
		}

		/// <summary>
		/// ��������� ����� �� ������ ��������
		/// </summary>
		/// <param name="attachment"></param>
		/// <returns></returns>
		public Stream OpenWrite(FormAttachment attachment) {
			return _storage.OpenWrite(attachment);
		}

		/// <summary>
		/// ��������� ����� �� ������ ��������
		/// </summary>
		/// <param name="attachment"></param>
		/// <returns></returns>
		public Stream OpenRead(FormAttachment attachment) {
			return _storage.OpenRead(attachment);
		}
	}
}