using System.Collections.Generic;

namespace Zeta.Extreme.BizProcess.Themas {
	/// <summary>
	/// Расширенный источник свойст для тем
	/// </summary>
	public interface IBizCasePropertySource {
		/// <summary>
		/// Возвращает перекрытие свойств для 
		/// </summary>
		/// <param name="themacode"></param>
		/// <returns></returns>
		IDictionary<string, object> GetExtendedProperties(string themacode);
	}
}