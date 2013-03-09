#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IQueryDimension.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Comdiv.Model.Interfaces;

namespace Comdiv.Zeta.Model.ExtremeSupport {
	/// <summary>
	/// 	Описывает стандартное измерение запросов Zeta
	/// </summary>
	/// <typeparam name="TItem"> </typeparam>
	public interface IQueryDimension<TItem> : IWithCacheKey, IZetaQueryDimension
		where TItem : class, IWithCode, IWithId, IWithNewTags {
		/// <summary>
		/// 	Набор кодов элемента
		/// </summary>
		string[] Codes { get; set; }

		/// <summary>
		/// 	Ссылка на исходный объект
		/// </summary>
		TItem Native { get; set; }

		/// <summary>
		/// 	Множественный набор идентификаторов
		/// </summary>
		int[] Ids { get; set; }

		/// <summary>
		/// 	Тип формулы
		/// </summary>
		string FormulaType { get; set; }

		/// <summary>
		/// 	Проверяет первичность элемента запроса
		/// </summary>
		/// <returns> </returns>
		bool IsPrimary();

		/// <summary>
		/// 	Нормализует объект зоны
		/// </summary>
		/// <param name="session"> </param>
		/// <exception cref="NotImplementedException"></exception>
		void Normalize(ISession session);
		}
}