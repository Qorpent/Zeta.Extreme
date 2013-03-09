using Comdiv.Olap.Model;

namespace Comdiv.Zeta.Model.ExtremeSupport {
	/// <summary>
	/// Стандартный интерфейс измерения запроса Obj
	/// </summary>
	public interface IObjHandler : IQueryDimension<IZoneElement>
	{
		/// <summary>
		/// 	Тип зоны
		/// </summary>
		ObjType Type { get; set; }

		/// <summary>
		/// Режим работы с деталями на уровне первичных запросов, по умолчанию NONE - выбор остается за системой
		/// </summary>
		DetailMode DetailMode { get; set; }

		/// <summary>
		/// 	Шоткат для быстрой проверки что речь идет о предприятии
		/// </summary>
		bool IsForObj { get; }

		/// <summary>
		/// 	Шоткат для быстрой проверки что речь идет не о предприятии
		/// </summary>
		bool IsNotForObj { get; }

		/// <summary>
		/// 	Ссылка на реальный экземпляр старшего объекта
		/// </summary>
		IZetaMainObject ObjRef { get; }

		/// <summary>
		/// 	Простая копия зоны
		/// </summary>
		/// <returns> </returns>
		IObjHandler Copy();
	}
}