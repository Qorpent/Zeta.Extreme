namespace Comdiv.Zeta.Model.ExtremeSupport {
	/// <summary>
	/// Стандартный описатель измерения "колонка"
	/// </summary>
	public interface IColumnHandler:IQueryDimension<IZetaColumn> {
		/// <summary>
		/// 	Простая копия условия на время
		/// </summary>
		/// <returns> </returns>
		IColumnHandler Copy();
	}
}