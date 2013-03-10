#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IMetaCache.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Model;

namespace Zeta.Extreme.Poco.Inerfaces {
	/// <summary>
	/// 	Интерфейс локального хранилища метаданных
	/// </summary>
	public interface IMetaCache {
		/// <summary>
		/// 	Родительский кэш
		/// </summary>
		IMetaCache Parent { get; set; }

		/// <summary>
		/// 	Получить объект из хранилища
		/// </summary>
		/// <typeparam name="T"> </typeparam>
		/// <param name="id"> </param>
		/// <returns> </returns>
		T Get<T>(object id) where T : class, IEntity;

		/// <summary>
		/// 	Сохранить объект в хранилище
		/// </summary>
		/// <typeparam name="T"> </typeparam>
		/// <param name="item"> </param>
		/// <returns> </returns>
		IMetaCache Set<T>(T item) where T : class, IEntity;
	}
}