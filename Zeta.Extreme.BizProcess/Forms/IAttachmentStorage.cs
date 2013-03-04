using System.Collections.Generic;
using System.IO;

namespace Zeta.Extreme.BizProcess.Forms {
	/// <summary>
	/// ��������� ��������������� ��������
	/// </summary>
	public interface IAttachmentStorage {
		/// <summary>
		/// ������������ ����� ����������� � ��������� ������ ������
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		IEnumerable<Attachment> Find(Attachment query);
		/// <summary>
		/// ��������� ��������� � ���������
		/// </summary>
		/// <param name="attachment"></param>
		void Save(Attachment attachment);

		/// <summary>
		/// ��������� ����� �� ������ ��������
		/// </summary>
		/// <param name="attachment"></param>
		/// <returns></returns>
		Stream OpenWrite(Attachment attachment);
		/// <summary>
		/// ��������� ����� �� ������ ��������
		/// </summary>
		/// <param name="attachment"></param>
		/// <returns></returns>
		Stream OpenRead(Attachment attachment);
	}
}