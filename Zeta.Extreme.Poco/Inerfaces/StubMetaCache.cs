using Comdiv.Model.Interfaces;
using Comdiv.Zeta.Model;

namespace Zeta.Extreme {
	public class StubMetaCache:IMetaCache {
		public static readonly StubMetaCache Default = new StubMetaCache();
		/// <summary>
		/// 	Получить объект из хранилища
		/// </summary>
		/// <typeparam name="T"> </typeparam>
		/// <param name="id"> </param>
		/// <returns> </returns>
		public T Get<T>(object id) where T : class, IEntityDataPattern {
			return null;
		}

		/// <summary>
		/// 	Сохранить объект в хранилище
		/// </summary>
		/// <typeparam name="T"> </typeparam>
		/// <param name="item"> </param>
		/// <returns> </returns>
		public IMetaCache Set<T>(T item) where T : class, IEntityDataPattern {
			return this;
		}

		/// <summary>
		/// Родительский кэш
		/// </summary>
		public IMetaCache Parent { get; set; }
	}
}