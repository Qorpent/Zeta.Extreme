using System;

namespace Zeta.Extreme.BizProcess.StateManagement.Attributes {
	/// <summary>
	///     ������� �������� ���� ������� �� ����������
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
		///     ��� ������� � ����������
		/// </summary>
		public string Code { get; private set; }
	}
}