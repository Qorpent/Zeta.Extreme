using System.Threading.Tasks;

namespace Zeta.Extreme {
	/// <summary>
	/// ќписывает самый простой, синхронизированный вариант доступа к сессии
	/// все запросы должны четко следовать один за другим
	/// </summary>
	public interface ISerialSession {
		/// <summary>
		/// √арантирует синхронный, последовательный доступ к сессии, вычисл€ет значение
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		QueryResult Eval(ZexQuery query);

		/// <summary>
		/// √арантирует синхронный, последовательный доступ к сессии, вычисл€ет значение
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		Task<QueryResult> EvalAsync(ZexQuery query);
	}
}