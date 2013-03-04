using System.Collections.Generic;
using System.IO;
using Zeta.Extreme.Form;
using Zeta.Extreme.Form.InputTemplates;

namespace Zeta.Extreme.BizProcess.Forms {
	/// <summary>
	/// ��������� ��������� ����������� ������ ����
	/// </summary>
	public interface IFormAttachmentStorage {
		/// <summary>
		/// ����� ��� �������������� �����, �������������� � ������ �������
		/// </summary>
		/// <returns></returns>
		IEnumerable<FormAttachment> GetAttachments(IFormSession session);

		/// <summary>
		/// ��������� ���������, ��������� � ������� �������
		/// </summary>
		/// <param name="session">������� ������ </param>
		/// <param name="attachment">�������������� �������</param>
		FormAttachment SaveAttachment(IFormSession session, Attachment attachment);

		/// <summary>
		/// ��������� ����� �� ������ ��������
		/// </summary>
		/// <param name="attachment"></param>
		/// <returns></returns>
		Stream OpenWrite(FormAttachment attachment);
		/// <summary>
		/// ��������� ����� �� ������ ��������
		/// </summary>
		/// <param name="attachment"></param>
		/// <returns></returns>
		Stream OpenRead(FormAttachment attachment);
	}
}