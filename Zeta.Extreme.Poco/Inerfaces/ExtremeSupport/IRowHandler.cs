namespace Comdiv.Zeta.Model.ExtremeSupport {
	/// <summary>
	/// ����������� ��������� ��������� Row
	/// </summary>
	public interface IRowHandler:IQueryDimension<IZetaRow> {


		/// <summary>
		/// 	True ���� ������� ������ - ������
		/// </summary>
		bool IsRef { get; }

		/// <summary>
		/// 	True ���� ������� ������ - ������
		/// </summary>
		bool IsSum { get; }

		/// <summary>
		/// 	������� ����� ������� �� ������
		/// </summary>
		/// <returns> </returns>
		IRowHandler Copy();

		/// <summary>
		/// 	����������� ������ � ���������
		/// </summary>
		/// <param name="session"> </param>
		/// <param name="column"> </param>
		void Normalize(ISession session, IZetaColumn column);
	}
}