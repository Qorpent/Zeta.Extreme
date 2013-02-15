#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : ZexQuery.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Comdiv.Zeta.Model;

namespace Zeta.Extreme {
	/// <summary>
	/// 	������������ ������� � Zeta
	/// </summary>
	/// <remarks>
	/// 	� ����������� ������ �� ������������ ����������
	/// 	����������� IQuery, IQueryBuilder, �������� ZexQuery
	/// 	������ � ������ ����������� � ����������� �������
	/// </remarks>
	public sealed class Query : CacheKeyGeneratorBase {
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
		/// 	������� �� �����
		/// </summary>
		public TimeHandler Time { get; set; }

		/// <summary>
		/// 	������� �� ������
		/// </summary>
		public RowHandler Row { get; set; }

		/// <summary>
		/// 	������� �� �������
		/// </summary>
		public ColumnHandler Col { get; set; }

		/// <summary>
		/// 	������� �� ������
		/// </summary>
		public ObjHandler Obj { get; set; }

		/// <summary>
		/// 	�������� ������
		/// </summary>
		public string Valuta { get; set; }

		/// <summary>
		/// 	�������� �������
		/// </summary>
		public IList<Query> FormulaDependency {
			get { return _formulaDependency ?? (_formulaDependency = new List<Query>()); }
		}

		/// <summary>
		/// 	�������� ������ �� ������
		/// </summary>
		public Session Session { get; set; }

		/// <summary>
		/// 	��������� "����������� �������"
		/// </summary>
		public bool IsPrimary {
			get { return Obj.IsPrimary() && Col.IsPrimary() && Row.IsPrimary(); }
		
		}

		/// <summary>
		/// 	������� ������� ��������� ����������
		/// </summary>
		public Task<QueryResult> GetResultTask { get; set; }

		/// <summary>
		/// 	���������� ���������
		/// </summary>
		public QueryResult Result { get; set; }

		/// <summary>
		/// 	��������� ���������� ������� � ����������
		/// </summary>
		public bool IsNotPrepared {
			get { return null == Result && null == GetResultTask; }
		}

		/// <summary>
		/// 	�������������� ��� �������, ������������� ��������
		/// </summary>
		public long UID;

		/// <summary>
		/// 	������������ ������ SQL
		/// </summary>
		public string SqlRequest;

		/// <summary>
		/// 	Back-reference to preparation tasks
		/// </summary>
		public Task PrepareTask { get; set; }

		/// <summary>
		/// Client processed mark
		/// </summary>
		public bool Processed { get; set; }

		/// <summary>
		/// ����������� ��� �������� ��������
		/// </summary>
		public IList<Tuple<decimal, Query>> SummaDependency {
			get { return _summaDependency ?? (_summaDependency = new List<Tuple<decimal, Query>>()); }
		
		}


		/// <summary>
		/// ��� ���������� �������
		/// </summary>
		public QueryEvaluationType EvaluationType;

		/// <summary>
		/// �������, ������� �������������� � ������� �� ���� ����������
		/// </summary>
		public IFormula AssignedFormula;

		/// <summary>
		/// Sign that primary was not set
		/// </summary>
		public bool HavePrimary;

		/// <summary>
		/// 	��������� ���������������� ������� � ����������
		/// </summary>
		/// <param name="timeout"> </param>
		public void WaitPrepare(int timeout=-1) {	
			while(null==PrepareTask) {
				Thread.Sleep(30);
			}
			if (PrepareTask != null) {
				if (!PrepareTask.IsCompleted) {
					if(timeout>0) {
						PrepareTask.Wait(timeout);
					}else {
						PrepareTask.Wait();
					}
				}
			}
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
		public Query Copy(bool deep = false) {
			var result = (Query) MemberwiseClone();
			result.PrepareTask = null;
			result.GetResultTask = null;
			result.Result = null;
			if(null!=TraceList) {
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
		public void Normalize(Session session = null) {
			var objt = Task.Run(() => Obj.Normalize(session ?? Session)); //������� �������� �� �� �����������
			Time.Normalize(session ?? Session);
			Col.Normalize(session ?? Session);
			var rowt = Task.Run(() => Row.Normalize(session ?? Session, Col.Native)); //��� ������� ������ ������� ��� ����			
			Task.WaitAll(objt, rowt);
			InvalidateCacheKey();
		}

		/// <summary>
		/// 	���������� ���-������
		/// </summary>
		public override void InvalidateCacheKey()
		{
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
		public QueryResult GetResult(int timeout =-1) {
			WaitResult(timeout);
			if (null != Result) {
				return Result;
			}
			if (null == GetResultTask) {
				throw new Exception("cannot retrieve result - no process or direct result attached");
			}

			if (GetResultTask.Status == TaskStatus.Faulted) {
				throw new Exception("cannot retrieve result - some problems int getresult task - faulted ", GetResultTask.Exception);
			}

			if (null != GetResultTask) {
				//��������� ������ ���������� ��������� ������������ ����������
				Result = GetResultTask.Result;
			}
			return Result;
		}

		/// <summary>
		/// 	��������� ������ (�� ������)
		/// </summary>
		/// <param name="zetaRow"> </param>
		/// <param name="selfcopy"> </param>
		/// <param name="rowcopy"> </param>
		public Query ToRow(IZetaRow zetaRow, bool selfcopy = false, bool rowcopy = false) {
			var q = this;
			if (selfcopy) {
				q = Copy();
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
		public void WaitResult(int timeout) {
			WaitPrepare(timeout);
			while(null == Result && null == GetResultTask) {
				Thread.Sleep(5);
			}
			if (null != GetResultTask) {
				if(GetResultTask.Status==TaskStatus.Created)
					if(IsPrimary) {
						Session.RunSqlBatch();
					}else {
						try {
							GetResultTask.Start();
						}
						catch {}
					}
				if(timeout>0) {
					GetResultTask.Wait(timeout);
				}else {
					GetResultTask.Wait();
				}
			}
		}

		/// <summary>
		/// 	����������� ���-������ (�������)
		/// </summary>
		public string CustomHashPrefix;

		private IList<Query> _formulaDependency;

		/// <summary>
		/// ������ ������
		/// </summary>
		public List<string> TraceList;

		private IList<Tuple<decimal, Query>> _summaDependency;
	}
}