using System;

namespace Zeta.Extreme.BizProcess.StateManagement.Attributes {
	/// <summary>
	///     Атрибут настройки информационной строки об ошибке
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
		///    Порядок проверки
		/// </summary>
		public int Index { get; private set; }
	}
}