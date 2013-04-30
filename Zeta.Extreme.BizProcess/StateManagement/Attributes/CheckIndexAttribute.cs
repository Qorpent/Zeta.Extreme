using System;

namespace Zeta.Extreme.BizProcess.StateManagement.Attributes {
	/// <summary>
	///     ������� ��������� �������������� ������ �� ������
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public class CheckIndexAttribute : StateAttributeBase
	{
		/// <summary>
		/// </summary>
		/// <param name="index"></param>
		public CheckIndexAttribute(int index)
		{
			Index = index;
		}

		/// <summary>
		///    ������� ��������
		/// </summary>
		public int Index { get; private set; }
	}
}