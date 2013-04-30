using Qorpent.Model;

namespace Zeta.Extreme.Model.Inerfaces {
	/// <summary>
	/// Описание бизнес-процесса (расширение тем)
	/// </summary>
	public interface IBizProcess:IEntity {
		/// <summary>
		/// Входящие процессы
		/// </summary>
		string InProcess { get; set; }
		/// <summary>
		///Связанная роль
		/// </summary>
		string Role { get; set; }
		/// <summary>
		/// Признак финальной формы
		/// </summary>
		bool IsFinal { get; set; }
		/// <summary>
		/// Корневые строки
		/// </summary>
		string RootRows { get; set; }
		/// <summary>
		/// Признак процесса
		/// </summary>
		string Process { get; set; }
	}
}