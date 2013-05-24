using System;
using System.Linq;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Querying;
using Zeta.Extreme.Model.MetaCaches;
namespace Zeta.Extreme {
	/// <summary>
	/// Простая имплементация измерения по контрагенту
	/// </summary>
	public sealed class ReferenceHandler : CacheKeyGeneratorBase, IReferenceHandler  {
		private string _contragents;
		private string _types;

		/// <summary>
		/// 	Функция непосредственного вычисления кэшевой строки
		/// </summary>
		/// <returns> </returns>
		protected override string EvalCacheKey() {
			return "ca:" + Contragents + ":" + Types;
		}
		/// <summary>
		/// Фильтр запроса по контрагентам
		/// </summary>
		/// <remarks>ZC-248 АССОИ-совместимая реализация </remarks>
		public string Contragents
		{
			get { return _contragents; }
			set
			{
				_contragents = value;
				InvalidateCacheKey();
			}
		}

		/// <summary>
		/// Фильтр по счетам
		/// </summary>
		
		public string Types
		{
			get { return _types; }
			set
			{
				_types = value;
				InvalidateCacheKey();
			}
		}

		/// <summary>
		/// Нормализация ищмерения для запроса
		/// </summary>
		/// <param name="query"></param>
		public void Normalize(IQuery query) {
			if (!string.IsNullOrWhiteSpace(Contragents))
			{
				var cache = null==query.Session?MetaCache.Default: query.Session.GetMetaCache();
				var ids = Contragents.SmartSplit().SelectMany(_ => cache.ResolveZoneAliasToObjectIds(_, null == query.Obj ? null : query.Obj.ObjRef)).Distinct().OrderBy(_ => _);
				Contragents = string.Join(",", ids);
			}
		}

		/// <summary>
		/// Копирование при копировании запроса
		/// </summary>
		/// <returns></returns>
		public IReferenceHandler Copy() {
			return MemberwiseClone() as IReferenceHandler;
		}

	}
}