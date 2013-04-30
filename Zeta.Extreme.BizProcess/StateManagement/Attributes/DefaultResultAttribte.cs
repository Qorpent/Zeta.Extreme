using System;

namespace Zeta.Extreme.BizProcess.StateManagement.Attributes {
	/// <summary>
	///     ������� ��������� ���������� �������� Allow (true) ��� Deny (false) � ������ ����������� ������� ������
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public class DefaultResultAttribte : StateAttributeBase
	{
		/// <summary>
		/// </summary>
		/// <param name="allow"></param>
		public DefaultResultAttribte(bool allow)
		{
			Allow = allow;
		}

		/// <summary>
		///     ��������� �� ������
		/// </summary>
		public bool Allow { get; private set; }
	}
}