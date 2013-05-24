using System;
using System.Linq;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Querying;
using Zeta.Extreme.Model.MetaCaches;
namespace Zeta.Extreme {
	/// <summary>
	/// ������� ������������� ��������� �� �����������
	/// </summary>
	public sealed class ReferenceHandler : CacheKeyGeneratorBase, IReferenceHandler  {
		private string _contragents;
		private string _types;

		/// <summary>
		/// 	������� ����������������� ���������� ������� ������
		/// </summary>
		/// <returns> </returns>
		protected override string EvalCacheKey() {
			return "ca:" + Contragents + ":" + Types;
		}
		/// <summary>
		/// ������ ������� �� ������������
		/// </summary>
		/// <remarks>ZC-248 �����-����������� ���������� </remarks>
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
		/// ������ �� ������
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
		/// ������������ ��������� ��� �������
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
		/// ����������� ��� ����������� �������
		/// </summary>
		/// <returns></returns>
		public IReferenceHandler Copy() {
			return MemberwiseClone() as IReferenceHandler;
		}

	}
}