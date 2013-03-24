using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeta.Extreme.Model.Extensions;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme.Primary
{
	/// <summary>
	/// Extension that evaluates <see cref="PrimaryQueryPrototype"/> of primary <see cref="Query"/> 
	/// </summary>
	public static class QueryPrototypeExtension
	{
		/// <summary>
		/// Retrieves prototype of the <paramref name="query"/>
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		public static PrimaryQueryPrototype GetPrototype(this IQuery query) {
			Contract.Requires<ArgumentNullException>(null!=query,"null query cannot have prototype");
			Contract.Requires<ArgumentException>(IsPrimary(query),"query must be primary");
			var result = new PrimaryQueryPrototype();
			CheckZetaEvalUsage(query, result);
			CheckAggregateEvalUsage(query, result);
			CheckDetailModeUsage(query, result);
			throw new NotImplementedException("пока не готово");
		}

		private static void CheckDetailModeUsage(IQuery query, PrimaryQueryPrototype result) {
			result.PreserveDetails = true;
			result.RequireDetails = false;

			if (ZoneType.Detail == query.Obj.Type) {
				result.RequireDetails = true;
				result.PreserveDetails = false;
				result.UseSum = false;
				return;
			}

			if (query.Obj.IsForObj && result.UseSum) {
				if (IsDetailAggregate(query)) {
					result.RequireDetails = true;
					result.PreserveDetails = false;
				}
				return;
			} 
		}

		private static void CheckAggregateEvalUsage(IQuery query, PrimaryQueryPrototype result) {
			if (IsAggregate(query)) {
				result.UseSum = true;
			}
		}

		private static bool IsAggregate(IQuery query) {
			if (query.Time.Periods != null && 1 < query.Time.Periods.Length) {
				return true;
			}
			if (IsDetailAggregate(query)) return true;
			return false;
		}

		private static bool IsDetailAggregate(IQuery query) {
			if (query.Obj.IsForObj) {
				if (!string.IsNullOrWhiteSpace(query.Obj.AltObjFilter)) {
					return true;
				}
				if (null != query.Row.Native) {
					if (PrimaryConstants.TAG_TRUE == query.Row.Native.ResolveTag(PrimaryConstants.TAG_USEDETAILS_PARAM)) {
						return true;
					}
				}
			}
			return false;
		}

		private static void CheckZetaEvalUsage(IQuery query, PrimaryQueryPrototype result) {
			if (query.Valuta != PrimaryConstants.VALUTA_NONE) {
				result.RequreZetaEval = true;
			}
		}

		private static bool IsPrimary(IQuery query) {
			Contract.Requires<ArgumentException>(null!=query.Obj && null!=query.Row && null!=query.Col);
			return query.Obj.IsPrimary() && query.Row.IsPrimary() && query.Col.IsPrimary();
		}
	}
}
