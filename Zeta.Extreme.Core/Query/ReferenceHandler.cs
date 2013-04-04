using System;
using System.Linq;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Model.Querying;
using Zeta.Extreme.Model.MetaCaches;
namespace Zeta.Extreme {
	/// <summary>
	/// ������� ������������� ��������� �� �����������
	/// </summary>
	public sealed class ReferenceHandler : CacheKeyGeneratorBase, IReferenceHandler  {
		private string _contragents;
		private string _accounts;

		/// <summary>
		/// 	������� ����������������� ���������� ������� ������
		/// </summary>
		/// <returns> </returns>
		protected override string EvalCacheKey() {
			return "ca:" + Contragents + ":" + Accounts;
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
		/// ������������ ��������� ��� �������
		/// </summary>
		/// <param name="session"></param>
		public void Normalize(ISession session) {
			Normalize(session, null);
		}

		/// <summary>
		/// ������������ ��������� ��� �������
		/// </summary>
		/// <param name="session"></param>
		/// <param name="handler"></param>
		public void Normalize(ISession session, IObjHandler handler) {
			if (!string.IsNullOrWhiteSpace(Contragents))
			{
				var cache = session.GetMetaCache();
				var ids = Contragents.SmartSplit().SelectMany(_ => cache.ResolveZoneAliasToObjectIds(_, null == handler ? null : handler.ObjRef)).Distinct().OrderBy(_ => _);
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