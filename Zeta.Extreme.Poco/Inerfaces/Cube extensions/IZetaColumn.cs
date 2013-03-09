#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IZetaColumn.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;

namespace Zeta.Extreme.Poco.Inerfaces {
	[Deprecated.Classic("ValueType")]
	[Deprecated.ForSearch("Колонка, показатель")]
	public interface IZetaColumn :
		IOlapColumn,

		IZetaQueryDimension,
		IWithMarkCache,
		IWithCells<IZetaCell, IZetaRow, IZetaColumn, IZetaMainObject, IZetaDetailObject>,
		IWithFixRules {
		string Valuta { get; set; }
		IDictionary<string, object> LocalProperties { get; set; }

		/// <summary>
		/// 	Поддержка режима "колонка как заместитель колсета"
		/// </summary>
		int Year { get; set; }

		/// <summary>
		/// 	Поддержка режима "колонка как заместитель колсета"
		/// </summary>
		int Period { get; set; }

		/// <summary>
		/// 	Поддержка режима "колонка как заместитель колсета"
		/// </summary>
		[global::Zeta.Extreme.Poco.Deprecated.NoMap] string ForeignCode { get; set; }

		string GetStaticMeasure(string format);
		string GetDynamicMeasure(IZetaRow source, string format);
		}
}