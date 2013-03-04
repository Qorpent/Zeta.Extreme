using System;
using System.Collections.Generic;

namespace Zeta.Extreme.BizProcess.Forms {
	/// <summary>
	/// �������, ������������� ��������
	/// </summary>
	public class Attachment {
		/// <summary>
		/// 
		/// </summary>
		public Attachment() {
			Metadata = new Dictionary<string, object>();
		}
		/// <summary>
		/// ���������� ���������� ��� �����
		/// </summary>
		public string Uid { get; set; }

		/// <summary>
		/// ��������
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// �����������
		/// </summary>
		public string Comment { get; set; }

		/// <summary>
		/// ���
		/// </summary>
		public string Type { get; set; }

		/// <summary>
		/// ����� ����������
		/// </summary>
		public string User { get; set; }

		/// <summary>
		/// ������ �����
		/// </summary>
		public DateTime Version { get; set; }

		/// <summary>
		/// ����-���
		/// </summary>
		public string MimeType { get; set; }

		/// <summary>
		/// ��� �����
		/// </summary>
		public string Md5 { get; set; }

		/// <summary>
		/// ������ �����
		/// </summary>
		public int Size { get; set; }

		/// <summary>
		/// �������
		/// </summary>
		public int Revision { get; set; }

		/// <summary>
		/// �������������� ���������
		/// </summary>
		public IDictionary<string, object> Metadata { get; private set; }
	}
}