using System;

namespace Zeta.Extreme.BizProcess.StateManagement.Attributes {
	/// <summary>
	///     јтрибут указани€ кода правила по регламенту
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
		///      од правила в регламенте
		/// </summary>
		public string Code { get; private set; }

		/// <summary>
		/// ќписание текста регламента из документа
		/// </summary>
		public string Description { get; set; }
	}
}