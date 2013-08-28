using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Developer.MetaStorage.Tree {
	/// <summary>
	/// 
	/// </summary>
	public interface ITreeExporter {
		/// <summary>
		/// Выполняет экспорт дерева в строку
		/// </summary>
		/// <param name="exportroot"></param>
		/// <param name="options"></param>
		/// <returns></returns>
		string ProcessExport(IZetaRow exportroot, TreeExporterOptions options = null);
	}
}