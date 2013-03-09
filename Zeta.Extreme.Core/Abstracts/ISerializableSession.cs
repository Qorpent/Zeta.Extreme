using System.Threading.Tasks;
using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme {
	/// <summary>
	/// Сериализуемая сессия
	/// </summary>
	public interface ISerializableSession:ISession {
		/// <summary>
		/// Сериальная синхронизация
		/// </summary>
		object SerialSync { get; }

		/// <summary>
		/// Задача для выполнения в асинхронном режиме из сериализованного доступа
		/// </summary>
		Task<QueryResult> SerialTask { get; set; }
	}
}