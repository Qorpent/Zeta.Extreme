using System.Text;

namespace Zeta.Extreme.Core {
	/// <summary>
	/// ������������ ������� � Zeta
	/// </summary>
	/// <remarks>
	/// � ����������� ������ �� ������������ ����������
	/// ����������� IQuery, IQueryBuilder, �������� ZexQuery
	/// ������ � ������ ����������� � ����������� �������
	/// </remarks>
	public sealed class ZexQuery : CacheKeyGeneratorBase {
		/// <summary>
		/// ����������� ���-������ (�������)
		/// </summary>
		public string CustomHashPrefix;

		/// <summary>
		/// ������� �� �����
		/// </summary>
		public TimeHandler Time { get; set; }
		/// <summary>
		/// ������� �� ������
		/// </summary>
		public RowHandler Row { get; set; }
		/// <summary>
		/// ������� �� �������
		/// </summary>
		public ColumnHandler Column { get; set; }
		/// <summary>
		/// ������� �� ������
		/// </summary>
		public ZoneHandler Zone { get; set; }
		/// <summary>
		/// �������� ������
		/// </summary>
		public string Valuta { get; set; }

		/// <summary>
		/// ������� ����������������� ���������� ������� ������
		/// </summary>
		/// <returns></returns>
		protected override string EvalCacheKey() {
			var sb = new StringBuilder();
			
			if(null!=CustomHashPrefix) {
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
	}
}