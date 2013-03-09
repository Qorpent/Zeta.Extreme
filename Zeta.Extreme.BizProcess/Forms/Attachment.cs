using System;
using System.Collections.Generic;
using Qorpent.Serialization;

namespace Zeta.Extreme.BizProcess.Forms {
	/// <summary>
	/// �������, ������������� ��������
	/// </summary>
	[Serialize]
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
		[SerializeNotNullOnly]
		public string Comment { get; set; }

		/// <summary>
		/// ���
		/// </summary>
		[SerializeNotNullOnly]
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
		[SerializeNotNullOnly]
		public string Hash { get; set; }

		/// <summary>
		/// ������ �����
		/// </summary>
		public long Size { get; set; }

		/// <summary>
		/// �������
		/// </summary>
		public int Revision { get; set; }

		/// <summary>
		/// �������������� ���������
		/// </summary>
		[SerializeNotNullOnly]
		public IDictionary<string, object> Metadata { get; private set; }
		/// <summary>
		/// ���������� �����
		/// </summary>
		public string Extension { get; set; }
	}
}