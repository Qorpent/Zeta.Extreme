using System;

namespace Zeta.Extreme.BizProcess.StateManagement.Attributes {
	/// <summary>
	///     Атрибут настройки информационной строки при нормальном статусе
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public class DefaultMessageAttribute : StateAttributeBase
	{
		/// <summary>
		/// </summary>
		/// <param name="message"></param>
		public DefaultMessageAttribute(string message)
		{
			Message = message;
		}

		/// <summary>
		///     Сообщение об ошибке
		/// </summary>
		public string Message { get; private set; }
	}
}