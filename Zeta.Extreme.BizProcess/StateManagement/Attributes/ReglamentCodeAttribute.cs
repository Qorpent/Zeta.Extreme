using System;

namespace Zeta.Extreme.BizProcess.StateManagement.Attributes {
	/// <summary>
	///     Атрибут указания кода правила по регламенту
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public class ReglamentCodeAttribute : StateAttributeBase
	{
		/// <summary>
		/// </summary>
		public ReglamentCodeAttribute(string code) {
			Code = code;
		}

		/// <summary>
		///     Код правила в регламенте
		/// </summary>
		public string Code { get; private set; }
	}
}