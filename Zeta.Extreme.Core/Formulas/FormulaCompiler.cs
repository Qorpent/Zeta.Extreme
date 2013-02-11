#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : FormulaCompiler.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;

namespace Zeta.Extreme {
	/// <summary>
	/// 	���������� ������ �� CSharp
	/// </summary>
	public class FormulaCompiler {
		/// <summary>
		/// 	������ �������� ����� ��� ������
		/// </summary>
		public const string MainTemplate = @"
using System;
using Comdiv.Zeta.Model;
using Comdiv.Zeta.Model.Implementation;
using Zeta.Extreme;
namespace Zeta.Extreme.DyncamicFormulas {
	[Formula(""{0}"")]
	public class Formula_{1} : {2} {
		public override decimal EvaluateNumber(){
			return Convert.ToDecimal (
				{3}
			);
		}
	}	
}
	";


		/// <summary>
		/// 	����� �� ���� ������ ������ � ����������� ��
		/// 	���������� ���� ������������� ��������
		/// </summary>
		/// <param name="formulaRequests"> </param>
		public void Compile(FormulaRequest[] formulaRequests) {
			throw new NotImplementedException();
		}
	}
}