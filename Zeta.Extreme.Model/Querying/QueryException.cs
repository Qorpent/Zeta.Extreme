using System;

namespace Zeta.Extreme.Model.Querying {
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class QueryException : Exception {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="query"></param>
		/// <param name="innerException"></param>
		public QueryException(IQuery query, Exception innerException) :base("error in query "+(null==query?"":query.GetCacheKey()),innerException) {
			this.Query = query;
		}

		public IQuery Query { get; private set; }
	}
}