#region LICENSE
// Copyright 2007-2013 Qorpent Team - http://github.com/Qorpent
// Supported by Media Technology LTD 
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//  
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// PROJECT ORIGIN: Zeta.Extreme.Core/Query.cs
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Zeta.Extreme.Model.Extensions;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme {
	/// <summary>
	/// 	������������ ������� � Zeta
	/// </summary>
	/// <remarks>
	/// 	� ����������� ������ �� ������������ ����������
	/// 	����������� IQuery, IQueryBuilder, �������� ZexQuery
	/// 	������ � ������ ����������� � ����������� �������
	/// </remarks>
	public sealed class Query : CacheKeyGeneratorBase, IQueryWithProcessing {
		/// <summary>
		/// 	����������� ������� �� ���������
		/// </summary>
		public Query() {
			Time = new TimeHandler();
			Row = new RowHandler();
			Col = new ColumnHandler();
			Obj = new ObjHandler();
			Reference = new ReferenceHandler();
			Currency = "NONE";
		}

		/// <summary>
		/// 	�������� �������
		/// </summary>
		public IList<IQuery> FormulaDependency {
			get { return _formulaDependency ?? (_formulaDependency = new List<IQuery>()); }
		}

		/// <summary>
		/// 	��������� "����������� �������"
		/// </summary>
		public bool IsPrimary {
			get { return Obj.IsPrimary() && Col.IsPrimary() && Row.IsPrimary(); }
		}


		/// <summary>
		/// 	���������� ���������
		/// </summary>
		public QueryResult Result { get; set; }


		/// <summary>
		/// 	Back-reference to preparation tasks
		/// </summary>
		public Task PrepareTask { get; set; }


		/// <summary>
		/// 	Client processed mark
		/// </summary>
		public bool Processed { get; set; }

		/// <summary>
		/// 	����������� ��� �������� ��������
		/// </summary>
		public IList<Tuple<decimal, IQuery>> SummaDependency {
			get { return _summaDependency ?? (_summaDependency = new List<Tuple<decimal, IQuery>>()); }
		}

		/// <summary>
		/// 	������� �� �����
		/// </summary>
		public ITimeHandler Time { get; set; }

		/// <summary>
		/// 	������� �� ������
		/// </summary>
		public IRowHandler Row { get; set; }

		/// <summary>
		///  ��������� �� �����������
		/// </summary>
		public IReferenceHandler Reference { get; set; }

		/// <summary>
		/// 	������� �� �������
		/// </summary>
		public IColumnHandler Col { get; set; }

		/// <summary>
		/// 	������� �� ������
		/// </summary>
		public IObjHandler Obj { get; set; }

		/// <summary>
		/// 	�������� ������
		/// </summary>
		public string Currency { get; set; }

		/// <summary>
		/// 	���������� ���-������
		/// </summary>
		public override void InvalidateCacheKey() {
			base.InvalidateCacheKey();
			Row.InvalidateCacheKey();
			Col.InvalidateCacheKey();
			Time.InvalidateCacheKey();
			Obj.InvalidateCacheKey();
		}

		/// <summary>
		/// 	������������ ������� ���������� �������
		/// </summary>
		/// <param name="timeout"> </param>
		/// <returns> </returns>
		/// <exception cref="Exception"></exception>
		public QueryResult GetResult(int timeout = -1) {
			lock (this) {
				WaitPrepare();
				
				if (null != Result) {
					if (null != Result.Error) {
						if (!(Result.Error is QueryException)) {
							Result.Error = new QueryException(this,Result.Error);
						}
					}
					return Result;
				}

				if (IsCircularDependency()) {
					Result = new QueryResult{IsComplete = false, Error = new Exception("circular dependency")};
				}
				
				if (EvaluationType == QueryEvaluationType.Summa && null == Result) {
					return GetSummaResult();
				}

				if (EvaluationType == QueryEvaluationType.Formula && null == Result) {
					return GetFormulaResult();
				}
				WaitResult(timeout);
				if (null != Result) {
					return Result;
				}
				return Result;
			}
		}

		private bool? _circular;
		/// <summary>
		/// ���������, ��� ������ ����� ����������� �����������
		/// </summary>
		public bool IsCircularDependency() {
			if (null == _circular) {
				_circular = EvalIsCircularDependency();
			}
			return _circular.Value;
		}

		private bool EvalIsCircularDependency() {
			if (IsPrimary) return false;
			var result = false;
			bool evalIsCircularDependency;
			if (CheckCachedDataForFormula(out evalIsCircularDependency)) {
				result = evalIsCircularDependency;
			}
			else {
				result = GetAllDependencies().Any(_ => _ == this);
			}
//if (!result) { //���������� ���������� �� ���� ������ ����������� ��� �� 
//				foreach (var query in GetAllDependencies().ToArray()) {
//					if (query is Query) {
//						//���� �� �� ���������� ����������� ������������ ���
//						((Query) query)._circular = false;
//					}
//				}
//			}
			return result;

		}
		private IEnumerable<Query> GetFirstLevelDependeny() {
			if (null != _formulaDependency) {
				foreach (var query in _formulaDependency.OfType<Query>()) {
					yield return query;
				}
			}
			if (null != _summaDependency)
			{
				foreach (var query in _summaDependency.Select(_=>_.Item2).OfType<Query>())
				{
					yield return query;
				}
			}
		}
		private bool CheckCachedDataForFormula(out bool evalIsCircularDependency) {
			evalIsCircularDependency = false;
			bool hasresult = true;
			bool hascircular = false;
			
				foreach (var q in GetFirstLevelDependeny()) {
					if (null != q) {
						if (q._circular.HasValue) {
							if (q._circular.Value) {
								hascircular = true;
							}
						}
						else {
							hasresult = false;
						}
					}
				}
				
			
			if (hascircular)
			{
				evalIsCircularDependency = true;
				return true;
			}
			if (hasresult)
			{
				evalIsCircularDependency = false;
				return true;
			}
			return false;
		}

		/// <summary>
		/// ���������� ��� �����������
		/// </summary>
		/// <returns></returns>
		public IEnumerable<IQuery> GetAllDependencies() {
			if(IsPrimary)yield break;
			if (null != SummaDependency && 0 != SummaDependency.Count) {
				foreach (var tuple in SummaDependency) {
					var query = tuple.Item2;
					yield return query;
				}
				//only if first level clean go to next
				foreach (var tuple in SummaDependency) {
					var query = tuple.Item2 as Query;
					if (query != this) { //prevent circular
						var deps = query.GetAllDependencies();
						foreach (var dep in deps) {
							yield return dep;
						}
					}
				}
			}
			if (null != FormulaDependency && 0 != FormulaDependency.Count) {
				foreach (var query in FormulaDependency)
				{
					yield return query;
				}
				//only if first level clean go to next
				foreach (Query query in FormulaDependency)
				{
					if (null != query) {
						if (query != this) {
							//prevent circular
							var deps = query.GetAllDependencies();
							foreach (var dep in deps) {
								yield return dep;
							}
						}
					}
				}
			}
		}

		private QueryResult GetFormulaResult() {
			AssignedFormula.Init(this);
			try {
				Result = AssignedFormula.Eval();
			}
			finally {
				AssignedFormula.CleanUp();
				//FormulaStorage.Default.Return(key, formula);
			}
			return Result;
		}

		private QueryResult GetSummaResult() {
			
			var subresults = SummaDependency.Select(sq => new {sq, val = sq.Item2.GetResult()}).ToArray();
			var fsterror = subresults.FirstOrDefault(_ => null != _.val.Error);
			if (null != fsterror) {		
				Result = new QueryResult {IsComplete = false, Error = new QueryException(this, fsterror.val.Error)};
			}
			else {
				var result =
					(subresults
						.Where(@t => null != @t.val)
						.Select(@t => @t.val.NumericResult*@t.sq.Item1)).
						Sum();
				Result = new QueryResult {IsComplete = true, NumericResult = result};
			}
			return Result;
		}


		/// <summary>
		/// 	��������� ���������������� ������� � ����������
		/// </summary>
		/// <param name="timeout"> </param>
		public void WaitPrepare(int timeout = -1) {
			while (PrepareState.Prepared != PrepareState) {
				if (PrepareTask != null) {
					if (!PrepareTask.IsCompleted) {
						PrepareTask.Wait();
					}
				}
				else {
					Thread.Sleep(20);
				}
			}
			PrepareTask = null;
		}


		/// <summary>
		/// 	������� ����������������� ���������� ������� ������
		/// </summary>
		/// <returns> </returns>
		protected override string EvalCacheKey() {
			var sb = new StringBuilder();

			if (null != CustomHashPrefix) {
				sb.Append('/');
				sb.Append(CustomHashPrefix);
			}
			sb.Append('/');
			sb.Append(null == Obj ? "NOOBJ" : Obj.GetCacheKey());
			sb.Append('/');
			sb.Append(null == Row ? "NOROW" : Row.GetCacheKey());
			sb.Append('/');
			sb.Append(null == Col ? "NOCOL" : Col.GetCacheKey());
			sb.Append('/');
			sb.Append(null == Time ? "NOTIME" : Time.GetCacheKey());
			sb.Append('/');
			sb.Append(string.IsNullOrWhiteSpace(Currency) ? "NOVAL" : "VAL:" + Currency);
			sb.Append('/');
			sb.Append(Reference.GetCacheKey());
			return sb.ToString();
		}

		/// <summary>
		/// 	������� ����� ������� �� �����
		/// </summary>
		/// <param name="deep"> ���� ��, �� ������ ����� ��������� ��������� </param>
		/// <returns> </returns>
		public IQuery Copy(bool deep = false) {
			var result = (Query) MemberwiseClone();
			result.PrepareTask = null;
			result.Result = null;
			result.EvaluationType = QueryEvaluationType.Unknown;
			result._summaDependency = null;
			result._formulaDependency = null;
			result.AssignedFormula = null;
			result.PrepareState = PrepareState.None;

			if (null != TraceList) {
				result.TraceList = new List<string>();
			}
			if (deep) {
				result.Col = result.Col.Copy();
				result.Row = result.Row.Copy();
				result.Time = result.Time.Copy();
				result.Obj = result.Obj.Copy();
				result.Reference = result.Reference.Copy();
			}

			return result;
		}


		/// <summary>
		/// 	����������� ��������� ������������
		/// </summary>
		public void Normalize(ISession session = null) {
			var objt = Task.Run(() => Obj.Normalize(session ?? Session)); //������� �������� �� �� �����������
			Time.Normalize(session ?? Session);
			Col.Normalize(session ?? Session);
			ResolveTemporalCustomCodeBasedColumns(session);
			Row.Normalize(session ?? Session, Col.Native); //��� ������� ������ ������� ��� ����			
			objt.Wait();
			AdaptDetailModeForDetailBasedSubtrees();
			AdaptExRefLinkSourceForColumns(session);
			Reference.Normalize(session);
			InvalidateCacheKey();
		}

		private void AdaptExRefLinkSourceForColumns(ISession session) {
			if (null != Col.Native  && null != Row.Native && null!=Col.Tag && null!=Row.Tag) {
				if (Col.Tag.Contains("/linkcol")) {
					var resolvedCode = Row.Native.GetRedirectColCode(Col.Native);
					if (resolvedCode != Col.Code) {
						Col.Native = (session ?? Session).GetMetaCache().Get<IZetaColumn>(resolvedCode);
					}
				}
			}
		}

		private void AdaptDetailModeForDetailBasedSubtrees() {
//������� ������������� ���� ��� �������� �� ������� �� �����
			if (Obj.IsForObj && Row.Native.ResolveTag("usedetails") == "1") {
				Obj.DetailMode = DetailMode.SafeSumObject;
			}
		}

		private void ResolveTemporalCustomCodeBasedColumns(ISession session) {
			while (null != Col.Native && !string.IsNullOrWhiteSpace(Col.Native.ForeignCode)) {
				var _c = Col;
				Col = new ColumnHandler {Code = _c.Native.ForeignCode};
				if (0 != _c.Native.Year || 0 != _c.Native.Period) {
					Time = new TimeHandler {Year = _c.Native.Year, Period = _c.Native.Period};
				}
				Col.Normalize(session ?? Session);
			}
		}

		/// <summary>
		/// 	��������� ������ (�� ������)
		/// </summary>
		/// <param name="zetaRow"> </param>
		/// <param name="selfcopy"> </param>
		/// <param name="rowcopy"> </param>
		public IQuery ToRow(IZetaRow zetaRow, bool selfcopy = false, bool rowcopy = false) {
			var q = this;
			if (selfcopy) {
				q = (Query)Copy();
			}
			if (rowcopy || selfcopy) {
				q.Row = q.Row.Copy();
			}
			q.Row.Native = zetaRow;
			q.InvalidateCacheKey();
			return q;
		}

		/// <summary>
		/// 	������������� ����������
		/// </summary>
		/// <param name="timeout"> </param>
		public void WaitResult(int timeout =-1) {
			WaitPrepare(timeout);
			if (IsPrimary && null == Result) {
				Session.WaitForPrimary(timeout);
			}
		}

		/// <summary>
		/// 	�������, ������� �������������� � ������� �� ���� ����������
		/// </summary>
		public IFormula AssignedFormula { get; set; }

		/// <summary>
		/// 	����������� ���-������ (�������)
		/// </summary>
		public string CustomHashPrefix;

		/// <summary>
		/// 	��� ���������� �������
		/// </summary>
		public QueryEvaluationType EvaluationType { get; set; }

		/// <summary>
		/// 	Sign that primary was not set
		/// </summary>
		public bool HavePrimary { get; set; }

		/// <summary>
		/// 	������ �� ����������
		/// </summary>
		public PrepareState PrepareState { get; set; }

		/// <summary>
		/// 	�������� ������ �� ������
		/// </summary>
		public ISession Session { get; set; }

		/// <summary>
		/// 	������������ ������ SQL
		/// </summary>
		public string SqlRequest;

		/// <summary>
		/// 	������ ������
		/// </summary>
		public List<string> TraceList;

		/// <summary>
		/// 	�������������� ��� �������, ������������� ��������
		/// </summary>
		public long Uid { get; set; }

		private List<IQuery> _formulaDependency;

		private List<Tuple<decimal, IQuery>> _summaDependency;
	}
}