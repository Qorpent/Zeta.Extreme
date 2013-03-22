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
			Valuta = "NONE";
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
		public string Valuta { get; set; }

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
					return Result;
				}
				if (EvaluationType == QueryEvaluationType.Summa && null == Result) {
					var result =
						(from sq in SummaDependency let val = sq.Item2.GetResult() where null != val select val.NumericResult*sq.Item1).
							Sum();
					Result = new QueryResult {IsComplete = true, NumericResult = result};
					return Result;
				}

				if (EvaluationType == QueryEvaluationType.Formula && null == Result) {
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

				WaitResult(timeout);
				if (null != Result) {
					return Result;
				}
				return Result;
			}
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
			sb.Append(string.IsNullOrWhiteSpace(Valuta) ? "NOVAL" : "VAL:" + Valuta);

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