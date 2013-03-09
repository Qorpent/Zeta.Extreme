#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IFormDataValidator.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;
using Qorpent.Mvc;
using Zeta.Extreme.Form.InputTemplates;
using Zeta.Extreme.Poco.Inerfaces;

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