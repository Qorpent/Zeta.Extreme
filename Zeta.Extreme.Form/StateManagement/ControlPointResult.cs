#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : ControlPointResult.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Zeta.Extreme.BizProcess.Themas;
using Zeta.Extreme.Meta;
using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme.Form.StateManagement {
	/// <summary>
	/// 	��������� ������ ����������� �����
	/// </summary>
	public class ControlPointResult {
		/// <summary>
		/// 	���������� ������
		/// </summary>
		public IZetaRow Row { get; set; }

		/// <summary>
		/// 	����������� �������
		/// </summary>
		public ColumnDesc Col { get; set; }

		/// <summary>
		/// 	�������� ��������
		/// </summary>
		public decimal Value { get; set; }

		/// <summary>
		/// 	�������� ���������� ����������� �����
		/// </summary>
		public bool IsValid {
			get { return Value == 0; }
		}

		/// <summary>
		/// 	������ �� �������� ������, ��������� ��������� ��������� ��������
		/// </summary>
		public Query Query { get; set; }
	}
}