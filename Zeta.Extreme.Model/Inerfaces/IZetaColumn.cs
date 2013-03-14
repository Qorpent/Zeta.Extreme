#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IZetaColumn.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;
using Zeta.Extreme.Model.Deprecated;
using Zeta.Extreme.Model.PocoClasses;


namespace Zeta.Extreme.Model.Inerfaces {
	[Classic("ValueType")]
	[ForSearch("�������, ����������")]
	public interface IZetaColumn : IZetaQueryDimension,
		IWithMarkCache, IWithMeasure {
		string Valuta { get; set; }
		IDictionary<string, object> LocalProperties { get; set; }
		/// <summary>
		/// 	��������� ������ "������� ��� ����������� �������"
		/// </summary>
		int Year { get; set; }
		/// <summary>
		/// 	��������� ������ "������� ��� ����������� �������"
		/// </summary>
		int Period { get; set; }
		/// <summary>
		/// 	��������� ������ "������� ��� ����������� �������"
		/// </summary>
		[NoMap] string ForeignCode { get; set; }

		ValueDataType DataType { get; set; }
		string DataTypeDetail { get; set; }
		string GetStaticMeasure(string format);
		string GetDynamicMeasure(IZetaRow source, string format);
		}
}