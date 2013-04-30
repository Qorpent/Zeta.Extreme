using System;

namespace Zeta.Extreme.BizProcess.StateManagement.Attributes {
	/// <summary>
	///     Атрибут настройки результата проверки Allow (true) или Deny (false) в случае прохождения входных фильтр
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
		///     Сообщение об ошибке
		/// </summary>
		public bool Allow { get; private set; }
	}
}