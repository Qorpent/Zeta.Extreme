using System;
using System.Threading.Tasks;
using System.Xml.Linq;
using Zeta.Extreme.Form;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// Интерфейс класса для сохранения данных форм
	/// </summary>
	public interface IFormSessionDataSaver {
		/// <summary>
		/// Метод 
		/// </summary>
		/// <param name="session"></param>
		/// <param name="savedata"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		Task<SaveResult> BeginSave(IFormSession session, XElement savedata);

		/// <summary>
		/// Текущая стадия процесса сохранения
		/// </summary>
		SaveStage Stage { get; set; }

		/// <summary>
		/// Последняя возникшая ошибка
		/// </summary>
		Exception Error { get; set; }
	}
}