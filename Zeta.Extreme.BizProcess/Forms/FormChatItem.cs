using System;
using System.Collections.Generic;
using Qorpent.Serialization;

namespace Zeta.Extreme.BizProcess.Forms {
	/// <summary>
	/// ������� ������ � ���
	/// </summary>
	[Serialize]
	public class FormChatItem {
		/// <summary>
		/// 
		/// </summary>
		public FormChatItem() {
			Userdata  = new Dictionary<string, object>();
		}
		/// <summary>
		/// ���������� ������������� ������
		/// </summary>
		public string Id { get; set; }
		/// <summary>
		/// ������������ ������
		/// </summary>
		public string User { get; set; }
		/// <summary>
		/// ����� ����c�
		/// </summary>
		public DateTime Time { get; set; }
		/// <summary>
		/// ����� ���������
		/// </summary>
		public string Text { get; set; }
		/// <summary>
		/// ��� �����
		/// </summary>
		public string FormCode { get; set; }
		/// <summary>
		/// �����������
		/// </summary>
		public int ObjId { get; set; }
		/// <summary>
		/// ���
		/// </summary>
		public int Year { get; set; }
		/// <summary>
		/// ������
		/// </summary>
		public int Period { get; set; }
		/// <summary>
		/// ��� ���������
		/// </summary>
		public string Type { get; set; }

		/// <summary>
		/// �������������� ������
		/// </summary>
		[Serialize]
		[SerializeNotNullOnly]
		public IDictionary<string,object> Userdata { get; set; }
	}
}