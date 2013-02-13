#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : IFormDataValidator.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;
using Comdiv.MVC;
using Comdiv.Zeta.Model;
using Zeta.Extreme.Form.InputTemplates;

namespace Zeta.Extreme.Form.SaveSupport {
	/// <summary>
	/// 	��������� ����������� ������� ����������� ������
	/// </summary>
	public interface IFormDataValidator {
		/// <summary>
		/// 	��������� �������� ������� �����, ���������� ��������� ��������
		/// </summary>
		/// <param name="targetResult"> </param>
		/// <param name="request"> </param>
		/// <param name="cells"> </param>
		/// <returns> </returns>
		ValidationResult Validate(ValidationResult targetResult, InputTemplateRequest request,
		                          IEnumerable<IZetaCell> cells);
	}
}