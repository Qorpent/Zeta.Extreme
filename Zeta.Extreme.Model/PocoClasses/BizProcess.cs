using Qorpent.Model;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Model {
	/// <summary>
	/// Описатель бизнес-процесса в БД, расширение тем
	/// </summary>
	public class BizProcess : Entity, IBizProcess {
		/// <summary>
		/// Входящие процессы
		/// </summary>
		public string InProcess { get; set; }

		/// <summary>
		///Связанная роль
		/// </summary>
		public string Role { get; set; }

		/// <summary>
		/// Признак финальной формы
		/// </summary>
		public bool IsFinal { get; set; }

		/// <summary>
		/// Корневые строки
		/// </summary>
		public string RootRows { get; set; }

		/// <summary>
		/// Признак процесса
		/// </summary>
		public string Process { get; set; }
	}
}