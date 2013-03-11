using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme.Model.Inerfaces {
	/// <summary>
	/// Вспомогательный интерфейс для сесси для доступа к хранимым данным
	/// </summary>
	public interface IWithDataServices {
		/// <summary>
		/// 	Расчетчик первичных данных
		/// </summary>
		IPrimarySource PrimarySource { get; set; }

		/// <summary>
		/// 	Локальный кэш объектных данных
		/// </summary>
		IMetaCache MetaCache { get; set; }

		/// <summary>
		/// Ожидает завершения задач, связанных с первичными данными
		/// </summary>
		/// <param name="timeout"></param>
		void WaitPrimarySource(int timeout = -1);
	}
}