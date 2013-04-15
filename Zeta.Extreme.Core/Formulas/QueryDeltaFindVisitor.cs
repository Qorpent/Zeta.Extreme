using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Zeta.Extreme {
	/// <summary>
	/// Визитор, который формирует список дельт по всему условному выражению
	/// </summary>
	public class QueryDeltaFindVisitor:ExpressionVisitor {
		/// <summary>
		/// Собирает вызовы дельт запросов по выражению
		/// </summary>
		/// <param name="e"></param>
		/// <returns></returns>
		public IEnumerable<QueryDelta> CollectDeltas(Expression e) {
			_result = new List<QueryDelta>();
			Visit(e);
			return _result.ToArray();
		} 
		private List<QueryDelta> _result;


		/// <summary>
		/// </summary>
		/// <param name="memberInit"></param>
		/// <returns></returns>
		protected override Expression VisitMemberInit(MemberInitExpression memberInit)
		{
			var result =(MemberInitExpression) base.VisitMemberInit(memberInit);
			if (result.NewExpression.Constructor.DeclaringType == typeof(QueryDelta))
			{
				LambdaExpression expr = Expression.Lambda(result);
				var d = expr.Compile();
				_result.Add( ((Func<QueryDelta>)d)());
			}
			return result;
		}
	}
}