namespace Comdiv.Zeta.Model.ExtremeSupport {
	/// <summary>
	/// ��������� ��������� �������
	/// </summary>
	public interface IQuery :IWithCacheKey{
		/// <summary>
		/// 	������� �� �����
		/// </summary>
		ITimeHandler Time { get; set; }

		/// <summary>
		/// 	������� �� ������
		/// </summary>
		IRowHandler Row { get; set; }

		/// <summary>
		/// 	������� �� �������
		/// </summary>
		IColumnHandler Col { get; set; }

		/// <summary>
		/// 	������� �� ������
		/// </summary>
		IObjHandler Obj { get; set; }

		/// <summary>
		/// 	�������� ������
		/// </summary>
		string Valuta { get; set; }
	}
}