namespace Zeta.Extreme.Model.Inerfaces {
	/// <summary>
	/// Вспомогательный интерфейс аттача и настройки на сессию
	/// </summary>
	public interface IWithSession {
		

		/// <summary>
		/// 	Обратная ссылка на сессию
		/// </summary>
		ISession Session { get; set; }
	}

	
}