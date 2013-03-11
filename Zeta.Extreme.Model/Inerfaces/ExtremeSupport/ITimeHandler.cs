#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : ITimeHandler.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme.Model.Inerfaces {
	/// <summary>
	/// 	Стандартный описатель измерения времени
	/// </summary>
	public interface ITimeHandler : IWithCacheKey {
		/// <summary>
		/// 	Год
		/// </summary>
		int Year { get; set; }

		/// <summary>
		/// 	Период
		/// </summary>
		int Period { get; set; }

		/// <summary>
		/// 	Набор из нескольких годов
		/// </summary>
		int[] Years { get; set; }

		/// <summary>
		/// 	Набор периодов
		/// </summary>
		int[] Periods { get; set; }

		/// <summary>
		/// 	Базовый год (для формовых запросов)
		/// </summary>
		int BaseYear { get; set; }

		/// <summary>
		/// 	Базовый период (для формовых запросов)
		/// </summary>
		int BasePeriod { get; set; }

		/// <summary>
		/// 	True если период приведен к константе
		/// </summary>
		/// <returns> </returns>
		bool IsPeriodDefined();

		/// <summary>
		/// 	True если год уже приведен к константе
		/// </summary>
		/// <returns> </returns>
		bool IsYearDefinied();

		/// <summary>
		/// 	Простая копия условия на время
		/// </summary>
		/// <returns> </returns>
		ITimeHandler Copy();

		/// <summary>
		/// 	Нормализует формульные года и периоды
		/// </summary>
		/// <param name="session"> </param>
		void Normalize(ISession session = null);
	}
}