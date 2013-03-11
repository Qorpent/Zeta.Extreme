#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : QueryProcessor.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Threading;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.Querying;
using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme {
	/// <summary>
	/// 	����������� ���������� ������������ ��������
	/// </summary>
	/// <remarks>
	/// 	����������� �������� � �������� �����������
	/// 	� ���, ��� ������ ��������� ������������,
	/// 	�������� � �������� �� ���� ��������� �������
	/// </remarks>
	public class QueryProcessor : IQueryPreparator {
		/// <summary>
		/// 	�������� �����������, ��������� � �������
		/// </summary>
		/// <param name="session"> </param>
		public QueryProcessor(Session session) {
			_session = session;
			_sumh = new StrongSumProvider();
		}

		/// <summary>
		/// 	��������� ���������� ������� � ����������
		/// 	����������� ����� ������������� � ��������
		/// </summary>
		/// <param name="query"> </param>
		public void Prepare(IQuery query) {
			var processableQuery = query as IQueryWithProcessing;
			if(null==processableQuery)return;
			if (processableQuery.PrepareState == PrepareState.InPrepare || processableQuery.PrepareState == PrepareState.Prepared)
			{
				return;
			}
			processableQuery.PrepareState = PrepareState.InPrepare;

			if (processableQuery.IsPrimary)
			{
				RegisterPrimaryRequest(processableQuery);
			}
			else {
				var mostpriority = GetMostPriorityNoPrimarySource(processableQuery);
				if (_sumh.IsSum(mostpriority)) {
					ExpandSum(processableQuery, mostpriority);
				}
				else {
					PrepareFormulas(processableQuery, mostpriority);
				}
			}
			processableQuery.PrepareState = PrepareState.Prepared;
		}

		private IZetaQueryDimension GetMostPriorityNoPrimarySource(IQuery query) {
			if (query.Obj.IsFormula || (query.Obj.IsForObj && _sumh.IsSum(query.Obj))) {
				return (query.Obj.ObjRef) ?? (IZetaQueryDimension) query.Obj;
			}
			if (query.Row.IsFormula || _sumh.IsSum(query.Row.Native)) {
				if (query.Col.IsFormula && query.Col.Native != null) {
					if (query.Col.Native.IsMarkSeted("DOSUM")) {
						return query.Row.Native ?? (IZetaQueryDimension) query.Row;
					}
				}
			}
			if (query.Col.IsFormula || _sumh.IsSum(query.Col.Native)) {
				return query.Col.Native ?? (IZetaQueryDimension) query.Col;
			}
			return query.Row.Native ?? (IZetaQueryDimension) query.Row;
		}

		private void PrepareFormulas(IQueryWithProcessing query, IZetaQueryDimension mostpriority) {
			query.EvaluationType = QueryEvaluationType.Formula;
			_session.StatIncQueryTypeFormula();
			var key = GetKey(mostpriority);
			var formula = FormulaStorage.Default.GetFormula(key, false);
			if (null == formula) {
				FormulaStorage.Default.Register(new FormulaRequest
					{Formula = mostpriority.Formula, Language = mostpriority.FormulaEvaluator, Key = key});
				formula = FormulaStorage.Default.GetFormula(key, false);
			}

			if (null == formula) {
				query.Result = new QueryResult {IsComplete = true, Error = new Exception("formula not found")};
				return;
			}
			query.AssignedFormula = formula;
			formula.Playback(query);
		}

		private static string GetKey(IZetaQueryDimension mostpriority) {
			var key = mostpriority.Code ?? Guid.NewGuid().ToString();
			if (mostpriority is RowHandler || mostpriority is IZetaRow) {
				key = "row:" + key;
			}
			else if (mostpriority is ColumnHandler || mostpriority is IZetaColumn) {
				if (mostpriority is ColumnHandler && null == ((ColumnHandler) mostpriority).Native) {
					key = "dyncol:" + mostpriority.Formula;
				}
				else {
					key = "col:" + key;
				}
			}
			else if (mostpriority is ObjHandler || mostpriority is IZetaMainObject) {
				key = "obj:" + key;
			}
			return key;
		}

		private void ExpandSum(IQuery query, IZetaQueryDimension mostpriority) {
			var peocessablequery = query as IQueryWithProcessing;
			if(null==peocessablequery)return;
			peocessablequery.EvaluationType = QueryEvaluationType.Summa;
			_session.StatIncQueryTypeSum();
			foreach (var r in _sumh.GetSumDelta(mostpriority)) {
				var sq = r.Apply(peocessablequery);
				sq = _session.Register(sq);
				if (null == sq) {
					continue;
				}
				peocessablequery.SummaDependency.Add(new Tuple<decimal, IQuery>(r.Multiplicator, sq));
			}

			if (peocessablequery.SummaDependency.Count == 0) {
				peocessablequery.Result = new QueryResult {IsComplete = true, NumericResult = 0m};
			}
		}

		/// <summary>
		/// 	������������ ��������� ������
		/// </summary>
		/// <param name="query"> </param>
		protected virtual void RegisterPrimaryRequest(IQuery query) {
			_session.StatIncQueryTypePrimary();
			_session.GetPrimarySource().Register(query);
		}

		private readonly ISession _session;
		private readonly StrongSumProvider _sumh;
	}
}