namespace Zeta.Extreme.FrontEnd.Session {
	/// <summary>
	/// Информация о статусе закрытия формы
	/// </summary>
	public class LockStateInfo {
		/// <summary>
		/// Зарезервированное понятия открытой формы
		/// </summary>
		public bool isopen;
		/// <summary>
		/// Текущий статус
		/// </summary>
		public string state;
		/// <summary>
		/// Признак возможности сохранения
		/// </summary>
		public bool cansave;
		/// <summary>
		/// Сообщение об ошибке сохранения
		/// </summary>
		public string message;
		/// <summary>
		/// Возможность блокировки формы
		/// </summary>
		public bool canblock;
	}
}