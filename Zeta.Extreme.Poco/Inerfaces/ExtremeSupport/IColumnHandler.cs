namespace Comdiv.Zeta.Model.ExtremeSupport {
	/// <summary>
	/// ����������� ��������� ��������� "�������"
	/// </summary>
	public interface IColumnHandler:IQueryDimension<IZetaColumn> {
		/// <summary>
		/// 	������� ����� ������� �� �����
		/// </summary>
		/// <returns> </returns>
		IColumnHandler Copy();
	}
}