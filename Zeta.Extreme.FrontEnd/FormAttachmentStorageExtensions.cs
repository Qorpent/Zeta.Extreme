using System;
using System.IO;
using System.Web;
using Qorpent.Applications;
using Qorpent.IO;
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
		/// <param name="filename"> </param>
		/// <param name="doctype">��� ������ (��������)</param>
		/// <param name="existeduid">������������ ������������� �����</param>
		/// <returns></returns>
		public static FormAttachment AttachHttpFile(this IFormAttachmentStorage storage, IFormSession session, HttpPostedFile file, string filename="", string doctype="" ,string existeduid="") {
			//�������������� ���� � ����������
			var attachment = SetupAttachment(session, file, filename, doctype, existeduid);
			//��������� ��������� �����
			attachment =storage.SaveAttachment(session, attachment);
			//��������� ����� ����� �������� ���� � ����� � �����
			const int BUFFER_SIZE = 500;
			using (var outstream = storage.Open(attachment,FileAccess.Write)) {
				file.InputStream.CopyTo(outstream,BUFFER_SIZE);
				outstream.Flush();
			}
			return attachment;
		}

		private static FormAttachment SetupAttachment(IFormSession session, HttpPostedFile file, string filename, string doctype,
		                                              string existeduid) {
//��������� ����, ��������� ����������� ������ �����
			var attachment = new FormAttachment(session, null, AttachedFileType.Default, false);
			//����������� ����� ����������� ��� � ���
			attachment = SetupFileInfo(attachment, file, filename);
			//������������� ���� ���� ������������ Uid (���� ��� ���������� ������������� �����)
			if (!string.IsNullOrWhiteSpace(existeduid)) {
				attachment.Uid = existeduid;
			}
			//���� ������, ������������� doctype
			if (!string.IsNullOrWhiteSpace(doctype)) {
				attachment.Type = doctype;
			}
			return attachment;
		}

		private static FormAttachment SetupFileInfo(FormAttachment attachment, HttpPostedFile file, string filename) {
			var srcname = file.FileName;
			var ext = Path.GetExtension(srcname);
			var mime = MimeHelper.GetMimeByExtension(ext);
			string realname = null;
			if (string.IsNullOrWhiteSpace(filename)) {
				realname = Path.GetFileNameWithoutExtension(file.FileName);
			}
			else {
				realname = filename;
			}
			attachment.Name = realname;
			attachment.Extension = ext;
			attachment.MimeType = mime;
			return attachment;
		}
	}
}