namespace Zeta.Extreme {
	/// <summary>
	/// ����������� �������� ��� �������� ��������������
	/// </summary>
	public class CompileErrorFormulaStub : DeltaFormulaBase {
		
		/// <summary>
		/// 	����� ��� ���������� ��� ������������ ������������ �������
		/// </summary>
		/// <returns> </returns>
		protected override object EvaluateExpression() {
			return "error in compilation";
		}
	}
}