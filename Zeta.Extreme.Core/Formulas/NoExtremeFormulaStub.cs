#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : NoExtremeFormulaStub.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Comdiv.Zeta.Model.ExtremeSupport;

namespace Zeta.Extreme {
	/// <summary>
	/// 	Специальная заглушка для возврата псевдозначения
	/// </summary>
	public class NoExtremeFormulaStub : FormulaBase {
		/// <summary>
		/// </summary>
		/// <param name="request"> </param>
		public override void SetContext(FormulaRequest request) {
			base.SetContext(request);
			Result = new QueryResult {IsComplete = false, Error = new NotSupportedException("formula ignored by noextreme tag")};
		}

		/// <summary>
		/// 	Метод для перекрытия при формировании динамической формулы
		/// </summary>
		/// <returns> </returns>
		protected override QueryResult InternalEval() {
			return Result;
		}

		/// <summary>
		/// 	Константный результат для данной формулы
		/// </summary>
		protected QueryResult Result;
	}
}