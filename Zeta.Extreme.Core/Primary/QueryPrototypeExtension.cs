using System;
using System.Diagnostics.Contracts;
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
			var result = new PrimaryQueryPrototype();
			CheckZetaEvalUsage(query,ref result);
			CheckAggregateEvalUsage(query,ref result);
			CheckDetailModeUsage(query,ref result);
			return result;
		}

		private static void CheckDetailModeUsage(IQuery query, ref PrimaryQueryPrototype result) {
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

		private static void CheckAggregateEvalUsage(IQuery query,ref PrimaryQueryPrototype result) {
			if (IsAggregate(query)) {
				result.UseSum = true;
				if (null != query.Time.Periods && 1 < query.Time.Periods.Length) {
					result.AggregatePeriod = true;
				}
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
				if (query.Obj.DetailMode == DetailMode.SafeSumObject || query.Obj.DetailMode == DetailMode.SumObject) {
					return true;
				}
				if (!string.IsNullOrWhiteSpace(query.Reference.Contragents)) {
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

		private static void CheckZetaEvalUsage(IQuery query, ref PrimaryQueryPrototype result) {
			if (query.Currency != PrimaryConstants.VALUTA_NONE) {
				result.UseZetaEval = true;
			}
		}

		
	}
}
