using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using Qorpent.Mvc;
using Qorpent.Mvc.Binding;
using Zeta.Extreme.BizProcess.Forms;
using Zeta.Extreme.FrontEnd.Helpers;
using Zeta.Extreme.Model.MetaCaches;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme.FrontEnd.Actions.Info {
	/// <summary>
	/// ƒействие получени€ технических данных по €чейке и истории ее формировани€
	/// </summary>
	[Action("zefs.evalstack", Role = "DEFAULT")]
	public class GetEvalStack : FormSessionActionBase {
		/// <summary>
		///  люч €чейки в сессии
		/// </summary>
		[Bind]
		protected string Key;

		private IQueryWithProcessing _myquery;
		private OutCell _cell;

		/// <summary>
		/// 	4 part of execution - all internal context is ready - authorize it with custom logic
		/// </summary>
		/// <exception cref="SecurityException">try access data of cell that not exists in requested session</exception>
		protected override void Authorize()
		{
			base.Authorize();
			if (MySession.Data.All(_ => _.i != Key))
			{
				throw new SecurityException("try access data of cell that not exists in requested session");
			}
		}

		/// <summary>
		/// 	Third part of execution - setup system-bound internal state here (called after validate, but before authorize)
		/// </summary>
		protected override void Prepare()
		{
			base.Prepare();
			_cell = MySession.Data.FirstOrDefault(_ => _.i == Key);
			if (null != _cell) {
				_myquery = (IQueryWithProcessing)_cell.query;
			}
		}

		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess() {
			if (null == _cell) {
				return "ƒанна€ €чейка сформирована с использованием нестандартного механизма, скорее всего из семейства " + MySession.Template.Thema.GetParameter("extension", "") + ", отладка не доступна";
			}
            if (Context.RenderName == "dot") {
                return new QueryGraphBuilder(_myquery) {ExcludePrimaryZeroes = true, ExcludeNonPrimaryZeroes = true};
            }
			return ConvertToStackInfo(_myquery);
		}

		private object ConvertToStackInfo( IQueryWithProcessing myquery,decimal mult=1, int level = 0) {
			if (level >= 10) {
				return "÷иркул€рна€ зависимость формулы";
			}
			if (null == myquery) {
				return "ƒанна€ €чейка сформирована с использованием нестандартного механизма, скорее всего из семейства "+MySession.Template.Thema.GetParameter("extension","")+", отладка не доступна";
			}
			return new
				{
					key = myquery.Uid,
					cachekey = myquery.GetCacheKey(),
					rowcode = myquery.Row.Code,
					rowname = myquery.Row.Name,
					isformula = myquery.EvaluationType==QueryEvaluationType.Formula,
					issum = myquery.EvaluationType==QueryEvaluationType.Summa,
					isprimary = myquery.EvaluationType==QueryEvaluationType.Primary||myquery.EvaluationType==QueryEvaluationType.Unknown,
					rowformula = myquery.Row.IsFormula? myquery.Row.Formula:"",
					colcode = myquery.Col.Code,
					colname = myquery.Col.Name,
					colformula = myquery.Col.IsFormula? myquery.Col.Formula:"",
					objid = myquery.Obj.Id,
					objname = myquery.Obj.Name,
					year = myquery.Time.Year,
					period = myquery.Time.Period,
					periods = myquery.Time.Periods==null?"":string.Join(",",myquery.Time.Periods),
					periodname = Periods.Get(myquery.Time.Period).Name,
					reference = myquery.Reference.GetCacheKey(),
					currency = myquery.Currency,
					type = myquery.EvaluationType,
					value = myquery.Result.NumericResult,
					iserror = null!=myquery.Result.Error,
					error = GetErrorObject(myquery.Result.Error),
					mult,
					subqueries = GetSubQueries(myquery, level)
				};
		}

		private object GetErrorObject(Exception error) {
			if (null == error) return error;
			if (error is QueryException) {
				return new
					{
						query = ((QueryException) error).Query.GetCacheKey(),
						realerror = GetErrorObject(error.InnerException)
					};

			}
			else {
				return new
					{
						message = error.Message,
						inner = GetErrorObject(error.InnerException)
					};
			}
		}

		/// <exception cref="Exception"></exception>
		private IEnumerable<object> GetSubQueries(IQueryWithProcessing myquery, int level) {
			if(myquery.EvaluationType==QueryEvaluationType.Primary ||  myquery.EvaluationType==QueryEvaluationType.Unknown)yield break;
			if (myquery.EvaluationType == QueryEvaluationType.Formula) {
				foreach (var query in myquery.FormulaDependency) {
					yield return  ConvertToStackInfo((IQueryWithProcessing) query,1,level:level+1);
				}
				
			}
			else if (myquery.EvaluationType == QueryEvaluationType.Summa) {
				foreach (var query in myquery.SummaDependency) {
					yield return ConvertToStackInfo((IQueryWithProcessing) query.Item2, query.Item1,level:level+1);
				}
			}
			else throw new Exception("unknown query type "+myquery.EvaluationType);
		}
	}

	
}