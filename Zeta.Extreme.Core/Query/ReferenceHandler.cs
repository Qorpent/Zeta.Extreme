using System;
using System.Linq;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Model.Querying;
using Zeta.Extreme.Model.MetaCaches;
namespace Zeta.Extreme {
	/// <summary>
	/// Простая имплементация измерения по контрагенту
	/// </summary>
	public sealed class ReferenceHandler : CacheKeyGeneratorBase, IReferenceHandler  {
		private string _contragents;
		private string _accounts;

		/// <summary>
		/// 	Функция непосредственного вычисления кэшевой строки
		/// </summary>
		/// <returns> </returns>
		protected override string EvalCacheKey() {
			return "ca:" + Contragents + ":" + Accounts;
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
		
		public string Accounts
		{
			get { return _accounts; }
			set
			{
				_accounts = value;
				InvalidateCacheKey();
			}
		}

		/// <summary>
		/// Нормализация ищмерения для запроса
		/// </summary>
		/// <param name="session"></param>
		public void Normalize(ISession session) {
			if (!string.IsNullOrWhiteSpace(Contragents))
			{
				var cache = session.GetMetaCache();
				var ids = Contragents.SmartSplit().SelectMany(cache.ResolveZoneAliasToObjectIds).Distinct().OrderBy(_ => _);
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