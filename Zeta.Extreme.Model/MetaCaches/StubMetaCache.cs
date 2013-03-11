#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : StubMetaCache.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Model;

namespace Zeta.Extreme.Model.MetaCaches {
	public class StubMetaCache : IMetaCache {
		public static readonly StubMetaCache Default = new StubMetaCache();

		/// <summary>
		/// 	Получить объект из хранилища
		/// </summary>
		/// <typeparam name="T"> </typeparam>
		/// <param name="id"> </param>
		/// <returns> </returns>
		public T Get<T>(object id) where T : class, IEntity {
			return null;
		}

		/// <summary>
		/// 	Сохранить объект в хранилище
		/// </summary>
		/// <typeparam name="T"> </typeparam>
		/// <param name="item"> </param>
		/// <returns> </returns>
		public IMetaCache Set<T>(T item) where T : class, IEntity {
			return this;
		}

		/// <summary>
		/// 	Родительский кэш
		/// </summary>
		public IMetaCache Parent { get; set; }
	}
}