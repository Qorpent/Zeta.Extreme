namespace Comdiv.Zeta.Web.InputTemplates {
	/// <summary>
	/// Правило на статус
	/// </summary>
	public class StateRule   {
		/// <summary>
		/// Текущая форма
		/// </summary>
		public string Current { get; set; }
		/// <summary>
		/// Целевая форма (?)
		/// </summary>
		public string Target { get; set; }
		/// <summary>
		/// Тип
		/// </summary>
		public string Type { get; set; }
		/// <summary>
		/// Действие
		/// </summary>
		public string Action { get; set; }
		/// <summary>
		/// Текущий статус
		/// </summary>
		public string CurrentState { get; set; }
		/// <summary>
		/// Результирующий статус
		/// </summary>
		public string ResultState { get; set; }
	}
}