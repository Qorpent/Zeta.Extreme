namespace Comdiv.Zeta.Model.ExtremeSupport {
	/// <summary>
	/// —тандартный описатель измерени€ Row
	/// </summary>
	public interface IRowHandler:IQueryDimension<IZetaRow> {


		/// <summary>
		/// 	True если целева€ строка - ссылка
		/// </summary>
		bool IsRef { get; }

		/// <summary>
		/// 	True если целева€ строка - ссылка
		/// </summary>
		bool IsSum { get; }

		/// <summary>
		/// 	ѕроста€ копи€ услови€ на строку
		/// </summary>
		/// <returns> </returns>
		IRowHandler Copy();

		/// <summary>
		/// 	Ќормализует ссылки и параметры
		/// </summary>
		/// <param name="session"> </param>
		/// <param name="column"> </param>
		void Normalize(ISession session, IZetaColumn column);
	}
}