using System;

namespace Zeta.Extreme.Benchmark {
	/// <summary>
	/// Тип результата
	/// </summary>
	[Flags]
	public enum ProbeResultType {
		/// <summary>
		/// Непопределенный
		/// </summary>
		Undefined = 1,
		/// <summary>
		/// Выполнен
		/// </summary>
		Success = 2,
		/// <summary>
		/// Проигнорирован
		/// </summary>
		Ignored = 4,
		/// <summary>
		/// Ошибка, исключение в процессе работы
		/// </summary>
		Error = 8,
	}
}