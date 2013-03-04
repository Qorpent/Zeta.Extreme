using System;
using System.Web;
using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// ��������������� ����� ��� ������ � ����������
	/// </summary>
	public static class FormAttachmentStorageExtensions {
		/// <summary>
		/// ��������������� ����� ��� ������� �������� ����� �� ������
		/// </summary>
		/// <param name="storage">������� ��������� </param>
		/// <param name="session">������</param>
		/// <param name="file">����, ���������� �� Http</param>
		/// <param name="doctype">��� ������ (��������)</param>
		/// <param name="existeduid">������������ ������������� �����</param>
		/// <returns></returns>
		public static FormAttachment AttachHttpFile(this IFormAttachmentStorage storage, IFormSession session, HttpPostedFile file, string doctype ,string existeduid="") {
			throw new NotImplementedException();
		}
	}
}