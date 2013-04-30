#region LICENSE
// Copyright 2007-2013 Qorpent Team - http://github.com/Qorpent
// Supported by Media Technology LTD 
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//  
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// PROJECT ORIGIN: Zeta.Extreme.FrontEnd/FormAttachmentStorageExtensions.cs
#endregion
using System.IO;
using System.Web;
using Qorpent.IO;
using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// 	��������������� ����� ��� ������ � ����������
	/// </summary>
	public static class FormAttachmentStorageExtensions {
		/// <summary>
		/// 	��������������� ����� ��� ������� �������� ����� �� ������
		/// </summary>
		/// <param name="storage"> ������� ��������� </param>
		/// <param name="session"> ������ </param>
		/// <param name="file"> ����, ���������� �� Http </param>
		/// <param name="filename"> </param>
		/// <param name="doctype"> ��� ������ (��������) </param>
		/// <param name="existeduid"> ������������ ������������� ����� </param>
		/// <returns> </returns>
		public static FormAttachment AttachHttpFile(this IFormAttachmentStorage storage, IFormSession session,
		                                            HttpPostedFileBase file, string filename = "", string doctype = "",
		                                            string existeduid = "") {
			//�������������� ���� � ����������
			var attachment = SetupAttachment(session, file, filename, doctype, existeduid);
			//��������� ��������� �����
			attachment = storage.SaveAttachment(session, attachment);
			//��������� ����� ����� �������� ���� � ����� � �����
			const int BUFFER_SIZE = 500;
			file.InputStream.Position = 0;
			using (var outstream = storage.Open(attachment, FileAccess.Write)) {
				file.InputStream.CopyTo(outstream, BUFFER_SIZE);
				outstream.Flush();
			}
			return attachment;
		}

		private static FormAttachment SetupAttachment(IFormSession session, HttpPostedFileBase file, string filename,
		                                              string doctype,
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

		private static FormAttachment SetupFileInfo(FormAttachment attachment, HttpPostedFileBase file, string filename) {
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