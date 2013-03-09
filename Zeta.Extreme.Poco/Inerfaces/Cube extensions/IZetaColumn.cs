#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IZetaColumn.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;
using Zeta.Extreme.Poco.Deprecated;

namespace Zeta.Extreme.Poco.Inerfaces {
	[Classic("ValueType")]
	[ForSearch("�������, ����������")]
	public interface IZetaColumn :
		IOlapColumn,
		IZetaQueryDimension,
		IWithMarkCache,
		IWithCells<IZetaCell, IZetaRow, IZetaColumn, IZetaMainObject, IZetaDetailObject>,
		IWithFixRules {
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

		string GetStaticMeasure(string format);
		string GetDynamicMeasure(IZetaRow source, string format);
		}
}