#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IZetaColumn.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;
using Qorpent.Model;
using Zeta.Extreme.Model.Deprecated;
using Zeta.Extreme.Model.Inerfaces.Bases;
using Zeta.Extreme.Model.Inerfaces.Partial;


namespace Zeta.Extreme.Model.Inerfaces {
	[Classic("ValueType")]
	[ForSearch("�������, ����������")]
	public interface IZetaColumn : IZetaQueryDimension,
		IWithMarkCache, IEntity, IWithFormula, IWithDataType, IWithMeasure {
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