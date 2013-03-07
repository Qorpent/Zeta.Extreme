using System;
using System.IO;
using Qorpent.Mvc;
using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// ������� ����� � ���� ����������� ����� ��� ��������
	/// </summary>
	public class FormAttachmentFileDescriptor : IFileDescriptor {
		private FormAttachment _attach;
		private IFormAttachmentStorage _storage;

		/// <summary>
		/// ������� ������� � �������������� 
		/// </summary>
		/// <param name="attach"></param>
		/// <param name="storage"></param>
		public FormAttachmentFileDescriptor(FormAttachment attach, IFormAttachmentStorage storage) {
			_attach = attach;
			_storage = storage;
		}

		/// <summary>
		/// 	���� (������ �����), ������� ���������� ��� �������
		/// </summary>
		public string Role {
			get { return ""; }
			set { }
		}

		/// <summary>
		/// 	��� ������� �������� � �����
		/// </summary>
		public string Name {
			get {
				if (string.IsNullOrWhiteSpace(Path.GetExtension(_attach.Name))) {
					return _attach.Name + "." + _attach.Extension;
				}
				return _attach.Name;
			}
			set { throw new NotImplementedException(); }
		}

		/// <summary>
		/// 	���������� �����
		/// </summary>
		public string Content {
			get { return null; }
			set { throw new NotImplementedException(); }
		}

		/// <summary>
		/// 	Mime-Type �����
		/// </summary>
		public string MimeType {
			get { return _attach.MimeType; }
			set { throw new NotImplementedException(); }
		}

		/// <summary>
		/// 	����� �����
		/// </summary>
		public int Length {
			get { return (int)_attach.Size; }
			set { throw new NotImplementedException(); }
		}

		/// <summary>
		/// 	����� ���������� ���������
		/// </summary>
		public DateTime LastWriteTime {
			get { return _attach.Version; }
			set { throw new NotImplementedException(); }
		}

		/// <summary>
		/// ������� ���� ��� ���� ������������ ����� ������������ �����
		/// </summary>
		public bool NeedDisposition {
			get { return true; }
			set { throw new NotImplementedException(); }
		}

		/// <summary>
		/// ������� ���������� �����
		/// </summary>
		public bool IsStream {
			get { return true; }
			set { throw new NotImplementedException(); }
		}

		/// <summary>
		/// ���������� ����� ������ �����
		/// </summary>
		/// <returns></returns>
		public Stream GetStream() {
			return _storage.Open(_attach, FileAccess.Read);
		}
	}
}