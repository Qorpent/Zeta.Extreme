namespace Zeta.Extreme {
	/// <summary>
	/// ����������� �������� ��� �������� ��������������
	/// </summary>
	public class CompileErrorFormulaStub : FormulaBase {
		/// <summary>
		/// ������������� �������� ������������� �������
		/// </summary>
		/// <param name="request"></param>
		public override void SetContext(FormulaRequest request)
		{
			base.SetContext(request);
			Result = new QueryResult {IsComplete = false, Error = request.ErrorInCompilation};
		}
		/// <summary>
		///����������� ��������� ������ �������
		/// </summary>
		protected QueryResult Result;

		/// <summary>
		/// 	����� ��� ���������� ��� ������������ ������������ �������
		/// </summary>
		/// <returns> </returns>
		protected override QueryResult InternalEval () {
			return Result;
		}
	}
}