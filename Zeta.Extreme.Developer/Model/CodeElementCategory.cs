using System;

namespace Zeta.Extreme.Developer.Model {
	/// <summary>
	/// Укрупненная категория элемента кода
	/// </summary>
	[Flags]
	public enum CodeElementCategory:long {
		/// <summary>
		/// Неопределенный	
		/// </summary>
		Undefined =0,
		/// <summary>
		/// Определение парамтера в целом
		/// </summary>
		ParamDef = CodeElementType.ParamDefLib | 
		           CodeElementType.ParamDefRoot |
		           CodeElementType.ReportParamDefLocalVar|
		           CodeElementType.ReportParamDefLocalParam|
		           CodeElementType.ParamInForm|
		           CodeElementType.ParamInParamset
		,
		/// <summary>
		/// Вариант использования параметра
		/// </summary>
		ParamUsage = CodeElementType.ParamAskInParamset|
		             CodeElementType.ParamAskInReportDef|
		             CodeElementType.ParamAskInReportSet|
		             CodeElementType.ParamAskInReportSetEx|
		             CodeElementType.ParamUseInParamset|
		             CodeElementType.ParamUseInReportDef|
		             CodeElementType.ParamUseInReportSet|
		             CodeElementType.ParamUseInReportSetEx|
		             CodeElementType.ParamUseReferenceInColset
		,
	}
}