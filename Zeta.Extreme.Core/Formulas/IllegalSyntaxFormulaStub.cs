#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IllegalSyntaxFormulaStub.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme {
	/// <summary>
	/// 	—пециальна€ заглушка дл€ возврата по неверно укомплектованным формулам
	/// </summary>
	public class IllegalSyntaxFormulaStub : FormulaBase {
		/// <summary>
		/// </summary>
		/// <param name="request"> </param>
		public override void SetContext(FormulaRequest request) {
			base.SetContext(request);
			Result = new QueryResult
				{IsComplete = false, Error = new InvalidOperationException("formula syntax error " + request.Formula)};
		}

		/// <summary>
		/// 	ћетод дл€ перекрыти€ при формировании динамической формулы
		/// </summary>
		/// <returns> </returns>
		protected override QueryResult InternalEval() {
			return Result;
		}

		/// <summary>
		/// 	 онстантный результат дл€ данной формулы
		/// </summary>
		protected QueryResult Result;
	}
}