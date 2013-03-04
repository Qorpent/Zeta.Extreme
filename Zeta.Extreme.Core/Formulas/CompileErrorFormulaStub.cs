#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : CompileErrorFormulaStub.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Comdiv.Zeta.Model.ExtremeSupport;

namespace Zeta.Extreme {
	/// <summary>
	/// 	����������� �������� ��� �������� ��������������
	/// </summary>
	public class CompileErrorFormulaStub : FormulaBase {
		/// <summary>
		/// 	������������� �������� ������������� �������
		/// </summary>
		/// <param name="request"> </param>
		public override void SetContext(FormulaRequest request) {
			base.SetContext(request);
			Result = new QueryResult {IsComplete = false, Error = request.ErrorInCompilation};
		}

		/// <summary>
		/// 	����� ��� ���������� ��� ������������ ������������ �������
		/// </summary>
		/// <returns> </returns>
		protected override QueryResult InternalEval() {
			return Result;
		}

		///<summary>
		///	����������� ��������� ������ �������
		///</summary>
		protected QueryResult Result;
	}
}