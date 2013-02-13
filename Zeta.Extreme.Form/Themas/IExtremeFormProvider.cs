using Zeta.Extreme.Form.InputTemplates;

namespace Zeta.Extreme.Form.Themas {
	/// <summary>
	/// Интерфейс загрузчика форм для Extreme
	/// </summary>
	public interface IExtremeFormProvider {
		/// <summary>
		/// Принудительная перезагрузка фабрики
		/// </summary>
		void Reload();

		/// <summary>
		/// Получить шаблон по коду
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		IInputTemplate Get(string code);
	}
}