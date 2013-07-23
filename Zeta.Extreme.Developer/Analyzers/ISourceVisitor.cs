using Zeta.Extreme.Developer.Model;

namespace Zeta.Extreme.Developer.Analyzers {
	/// <summary>
	/// Интерфейс посетителя кода
	/// </summary>
	public interface ISourceVisitor {
		/// <summary>
		/// Выполнить посещение источника
		/// </summary>
		/// <param name="source"></param>
		void Visit(Source source);
	}
}