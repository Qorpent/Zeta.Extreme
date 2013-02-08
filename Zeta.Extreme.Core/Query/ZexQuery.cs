#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : ZexQuery.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;
using System.Text;

namespace Zeta.Extreme {
	/// <summary>
	/// 	������������ ������� � Zeta
	/// </summary>
	/// <remarks>
	/// 	� ����������� ������ �� ������������ ����������
	/// 	����������� IQuery, IQueryBuilder, �������� ZexQuery
	/// 	������ � ������ ����������� � ����������� �������
	/// </remarks>
	public sealed class ZexQuery : CacheKeyGeneratorBase {
		/// <summary>
		/// ����������� ������� �� ���������
		/// </summary>
		public ZexQuery() {
			Time = new TimeHandler();
			Row = new RowHandler();
			Column = new ColumnHandler();
			Zone =new ZoneHandler();
			Valuta = "NONE";
		}
		/// <summary>
		/// 	������� �� �����
		/// </summary>
		public TimeHandler Time { get; set; }

		/// <summary>
		/// 	������� �� ������
		/// </summary>
		public RowHandler Row { get; set; }

		/// <summary>
		/// 	������� �� �������
		/// </summary>
		public ColumnHandler Column { get; set; }

		/// <summary>
		/// 	������� �� ������
		/// </summary>
		public ZoneHandler Zone { get; set; }

		/// <summary>
		/// 	�������� ������
		/// </summary>
		public string Valuta { get; set; }

		/// <summary>
		/// 	������� ����������������� ���������� ������� ������
		/// </summary>
		/// <returns> </returns>
		protected override string EvalCacheKey() {
			var sb = new StringBuilder();

			if (null != CustomHashPrefix) {
				sb.Append('/');
				sb.Append(CustomHashPrefix);
			}
			sb.Append('/');
			sb.Append(null == Zone ? "NOOBJ" : Zone.GetCacheKey());
			sb.Append('/');
			sb.Append(null == Row ? "NOROW" : Row.GetCacheKey());
			sb.Append('/');
			sb.Append(null == Column ? "NOCOL" : Column.GetCacheKey());
			sb.Append('/');
			sb.Append(null == Time ? "NOTIME" : Time.GetCacheKey());
			sb.Append('/');
			sb.Append(string.IsNullOrWhiteSpace(Valuta) ? "NOVAL" : "VAL:" + Valuta);

			return sb.ToString();
		}

		/// <summary>
		/// 	����������� ���-������ (�������)
		/// </summary>
		public string CustomHashPrefix;

		private IList<ZexQuery> _children;

		/// <summary>
		/// ������� ����� ������� �� �����
		/// </summary>
		/// <param name="deep">���� ��, �� ������ ����� ��������� ���������</param>
		/// <returns></returns>
		public ZexQuery Copy(bool deep = false) {
			var result = (ZexQuery) MemberwiseClone();
			if (deep) {
				result.Column = result.Column.Copy();
				result.Row = result.Row.Copy();
				result.Time = result.Time.Copy();
				result.Zone = result.Zone.Copy();

			}

			return result;
		}

		/// <summary>
		/// �������� �������
		/// </summary>
		public IList<ZexQuery> Children {
			get { return _children ?? (_children = new List<ZexQuery>()); }

		}

		/// <summary>
		/// ������������ ������
		/// </summary>
		public ZexQuery Parent { get; set; }

		/// <summary>
		/// �������� ������ �� ������
		/// </summary>
		public ZexSession Session { get; set; }
	}
}