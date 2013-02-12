using System;

namespace Zeta.Extreme {
	/// <summary>
	/// ����������� �������� ��� �������� ��������������
	/// </summary>
	public class NoExtremeFormulaStub : FormulaBase
	{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="request"></param>
		public override void SetContext(FormulaRequest request)
		{
			base.SetContext(request);
			Result = new QueryResult { IsComplete = false, Error = new NotSupportedException("formula ignored by noextreme tag") };
		}
		/// <summary>
		/// ����������� ��������� ��� ������ �������
		/// </summary>
		protected QueryResult Result;

		/// <summary>
		/// 	����� ��� ���������� ��� ������������ ������������ �������
		/// </summary>
		/// <returns> </returns>
		protected override QueryResult InternalEval()
		{
			return Result;
		}
	}
}