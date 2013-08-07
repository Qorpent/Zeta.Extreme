using Qorpent;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Model.MetaStorage {
	/// <summary>
	/// Настройки фильтрации дерева
	/// </summary>
	public class ExportTreeFilter {
		/// <summary>
		/// Регулярное выражение исключения узла
		/// </summary>
		public string ExcludeRegex { get; set; }
		/// <summary>
		/// Замена кода по регексу
		/// </summary>
		public ReplaceDescriptor CodeReplacer { get; set; }

		/// <summary>
		/// Выполняет фильтрацию дерева
		/// </summary>
		/// <param name="exportroot"></param>
		/// <returns></returns>
		public IZetaRow Execute(IZetaRow exportroot){
			throw new System.NotImplementedException();
		}
	}
}