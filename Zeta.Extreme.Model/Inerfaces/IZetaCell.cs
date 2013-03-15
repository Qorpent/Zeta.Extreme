#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IZetaCell.cs
// Project: Zeta.Extreme.Model
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Qorpent.Model;

namespace Zeta.Extreme.Model.Inerfaces {
	/// <summary>
	/// 	���������� ������ ����
	/// </summary>
	public interface IZetaCell :
		IWithUsr, IWithId, IWithVersion {
		/// <summary>
		/// ������
		/// </summary>
		IZetaRow Row { get; set; }
		/// <summary>
		/// �������
		/// </summary>
		IZetaColumn Column { get; set; }
		/// <summary>
		/// ������� ������
		/// </summary>
		IZetaMainObject Object { get; set; }
		/// <summary>
		/// ������� ������
		/// </summary>
		IZetaDetailObject Detail { get; set; }
		/// <summary>
		/// ����������
		/// </summary>
		IZetaMainObject AltObj { get; set; }
		
		/// <summary>
		/// ������
		/// </summary>
		int Period { get; set; }
		/// <summary>
		/// ���
		/// </summary>
		int Year { get; set; }
		/// <summary>
		/// ������ ����
		/// </summary>
		DateTime DirectDate { get; set; }

		/// <summary>
		/// ������ ������
		/// </summary>
		string Valuta { get; set; }
		
		/// <summary>
		/// �� �����������
		/// </summary>
		int? AltObjId { get; set; }
		/// <summary>
		/// �� ������
		/// </summary>
		int RowId { get; set; }
		/// <summary>
		/// �� �������
		/// </summary>
		int ColumnId { get; set; }
		/// <summary>
		/// �� �������
		/// </summary>
		int ObjectId { get; set; }
		/// <summary>
		/// �� ������
		/// </summary>
		int? DetailId { get; set; }

		/// <summary>
		/// ��������� ��������
		/// </summary>
		decimal NumericValue { get; set; }
		/// <summary>
		/// ��������� ��������
		/// </summary>
		string StringValue { get; set; }
		
		}
}