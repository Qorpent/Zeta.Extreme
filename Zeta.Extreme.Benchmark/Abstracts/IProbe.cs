using System.Collections.Generic;
using System.Threading.Tasks;

namespace Zeta.Extreme.Benchmark
{
	/// <summary>
	/// Интерфейс пробы
	/// </summary>
	public interface IProbe {
		/// <summary>
		/// Родительская проба
		/// </summary>
		IProbe Parent { get; set; }
		/// <summary>
		/// Дочерние пробы
		/// </summary>
		IList<IProbe> Children { get; }

		/// <summary>
		///  Инициализация пробы
		/// </summary>
		/// <param name="config"></param>
		void Initialize(IProbeConfig config = null);
		/// <summary>
		/// Асинхронное выполнение пробы
		/// </summary>
		/// <returns></returns>
		Task<IProbeResult> ExecuteAsync();
		/// <summary>
		/// Синхронное выполнение пробы
		/// </summary>
		/// <returns></returns>
		IProbeResult ExecuteSync();
		/// <summary>
		/// Акцессор к конфигу
		/// </summary>
		/// <returns></returns>
		IProbeConfig GetConfig();

	}
}
