using Comdiv.Model.Interfaces;
using Comdiv.Zeta.Model;

namespace Zeta.Extreme {
	public class StubMetaCache:IMetaCache {
		public static readonly StubMetaCache Default = new StubMetaCache();
		/// <summary>
		/// 	�������� ������ �� ���������
		/// </summary>
		/// <typeparam name="T"> </typeparam>
		/// <param name="id"> </param>
		/// <returns> </returns>
		public T Get<T>(object id) where T : class, IEntityDataPattern {
			return null;
		}

		/// <summary>
		/// 	��������� ������ � ���������
		/// </summary>
		/// <typeparam name="T"> </typeparam>
		/// <param name="item"> </param>
		/// <returns> </returns>
		public IMetaCache Set<T>(T item) where T : class, IEntityDataPattern {
			return this;
		}

		/// <summary>
		/// ������������ ���
		/// </summary>
		public IMetaCache Parent { get; set; }
	}
}