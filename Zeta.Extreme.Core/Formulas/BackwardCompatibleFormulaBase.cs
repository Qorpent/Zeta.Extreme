#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : BackwardCompatibleFormulaBase.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Linq;
using Comdiv.Extensions;
using Comdiv.Zeta.Model;

namespace Zeta.Extreme {
	/// <summary>
	/// 	Базовая формула для режима совместимости с BOO формулами предыдущего поколения
	/// </summary>
	public abstract class BackwardCompatibleFormulaBase : DeltaFormulaBase {
		/// <summary>
		/// шоткат для совместимости со старыми формулами
		/// </summary>
		protected Query q { get { return Query; } }
		/// <summary>
		/// шоткат для совместимости со старыми формулами
		/// </summary>
		
		protected Query query { get { return Query; } }
		/// <summary>
		/// акцессор к совместимому расширенному формуласету
		/// </summary>
		protected BackwardCompatibleMainFormulaSet f;
		/// <summary>
		/// Конструктор по умолчанию - инициирует часть старых сервисов
		/// </summary>
		public BackwardCompatibleFormulaBase() {
			f = new BackwardCompatibleMainFormulaSet(this);
		}


		/// <summary>
		/// Быстрый акцессор к году запроса
		/// </summary>
		public int year
		{
			get { return q.Time.Year; }
		}
		/// <summary>
		/// Проверяет соотвествие кодов колонки запроса
		/// </summary>
		/// <param name="codes"></param>
		/// <returns></returns>
		protected bool colin(params string[] codes)
		{
			return codes.Select(x => x.ToUpper()).Any(x => x.ToUpper() == q.Col.Code.ToUpper() || (q.Col.Native != null && q.Col.Native.Code.ToUpper() == x.ToUpper()));
			//return Array.IndexOf(codes, q.Column.Code) != -1;
		}
		/// <summary>
		/// Проверяет соотвтетсвие строки путям
		/// </summary>
		/// <param name="codes"></param>
		/// <returns></returns>
		protected bool pathin(params string[] codes)
		{
			if (q.Row.Native == null) return false;
			return codes.Any(x => q.Row.Native.Path.Contains("/" + x + "/"));
		}
		/// <summary>
		/// Проверяет принадлежность текущей строки списку
		/// </summary>
		/// <param name="codes"></param>
		/// <returns></returns>
		protected bool rowin(params string[] codes)
		{
			return Array.IndexOf(codes, q.Row.Code) != -1;
		}
		/// <summary>
		/// Не поддерживаемя на данный момент опция проверки группы предприятий или деталей
		/// </summary>
		/// <param name="groups"></param>
		/// <returns></returns>
		protected bool groupin(params string[] groups)
		{
			throw new NotSupportedException("на данный момент поддержка этой опции в Zeta.Extreme отсутвует");
			/*
			foreach (var g in groups)
			{
				if (q.Zone.GroupFilter == g) return true; // это вообще не понятно
				else if (q.Zone.Item != null && q.Zone.Item is IZetaObjectGroup)
				{
					if (q.Zone.Item.Code == g) return true;
				}
				else if (q.Zone.IsForOrg())
				{
					if (((IZetaMainObject)q.Zone.Item).GroupCache.Contains("/" + g + "/")) return true;
				}
			}
			return false;
			 */
		}
	/// <summary>
	/// Проверяет соотвествие периодов запроса набору
	/// </summary>
	/// <param name="periods"></param>
	/// <returns></returns>
		protected bool periodin(params int[] periods)
		{
			return Array.IndexOf(periods, q.Time.Period) != -1;
		}

		/// <summary>
		/// Проверяет на каком счете числится текущая деталь
		/// </summary>
		/// <param name="contostring"></param>
		/// <returns></returns>
		protected bool contoin(string contostring) {
			throw new NotSupportedException("на данный момент поддержка деталей и счетов в Zeta.Extreme отсутвует");
			/*
			var conts = contostring.split();
			if (0 == conts.Count) return true;


			if (this.query.Zone.DetailTypeFilter.hasContent())
			{
				var actconts = this.query.Zone.DetailTypeFilter.split();
				return actconts.All(conts.Contains);
			}
			else if (this.query.Zone.IsDetail())
			{
				var actconts = query.Zone.GetAllDetails().Select(x => x.Type.Code).Distinct();
				return actconts.All(conts.Contains);
			}
			else
			{
				var rowconto =
					string.Join(",",
								new[] { query.Row.Target }.Union(query.Row.Target.AllChildren).Select(x => TagHelper.Value(x.Tag, "casbill"))
									.Distinct().
									ToArray()).split().Distinct().ToArray();
				if (rowconto.Length > 0)
				{
					return rowconto.All(conts.Contains);
				}
			}
			return false;
			 */
		}

		private static readonly int[] months = new int[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 110, 111, 112 };
		private static readonly int[] aggs = new int[] { 22, 1, 24, 25, 2, 27, 28, 3, 210, 211, 4 };
		private static readonly int[] plans = new int[] { 301, 303, 306, 309, 251, 252, 253, 254 };
		private static readonly int[] ozhids = new int[] { 401, 403, 406, 409 };
		private static readonly int[] korrektives = new int[] { 31, 32, 33, 34, 311, 312, 313, 314, 321, 322, 323, 324, 331, 332, 333, 334, 341, 342, 343, 344 };

		/// <summary>
		/// относится ли период к месяцам
		/// </summary>
		protected bool ismonth
		{
			get { return Array.IndexOf(months, q.Time.Period) != -1; }
		}

		/// <summary>
		/// относится ли период к кварталам
		/// </summary>
		protected int monthInKvart
		{
			get { return f.monthInKvart(q.Time.Period); }
		}
		/// <summary>
		/// относится ли период к коррективам
		/// </summary>
		protected bool iskorrperiod
		{
			get { return Array.IndexOf(korrektives, q.Time.Period) != -1; }
		}
		/// <summary>
		/// относится ли период к суммовым
		/// </summary>
		protected bool issumperiod
		{
			get { return Array.IndexOf(aggs, q.Time.Period) != -1; }
		}

		/// <summary>
		/// относится ля период к плановым
		/// </summary>
		protected bool isplanperiod
		{
			get { return Array.IndexOf(plans, q.Time.Period) != -1; }
		}
		/// <summary>
		/// Относится ли период к ожидаемым
		/// </summary>
		protected bool isozhidperiod
		{
			get { return Array.IndexOf(ozhids, q.Time.Period) != -1; }
		}






		/// <summary>
		/// соответствие строки группе
		/// </summary>
		/// <param name="groups"></param>
		/// <returns></returns>
		protected bool rowgroupin(params string[] groups)
		{
			if (null == q.Row.Native) return false;
			var grps = q.Row.Native.Group.split(false, true, '/', ';');
			if (0 == grps.Count) return false;
			foreach (var g in groups)
			{
				if (grps.Any(x => x == g)) return true;
			}
			return false;
		}
		/// <summary>
		/// соответствие дерева группе
		/// </summary>
		/// <param name="groups"></param>
		/// <returns></returns>
		protected bool treegroupin(params string[] groups)
		{
			var current = q.Row.Native;
			if (null == current) return false;

			while (null != current)
			{

				var grps = current.Group.split(false, true, '/', ';');
				foreach (var g in groups)
				{
					if (grps.Any(x => x == g)) return true;
				}
				current = current.Parent;
			}
			return false;
		}
		/// <summary>
		/// соответствие строки метке
		/// </summary>
		/// <param name="marks"></param>
		/// <returns></returns>
		protected bool rowlabelin(params string[] marks)
		{
			if (null == q.Row.Native) return false;
			foreach (var m in marks)
			{
				if (q.Row.Native.IsMarkSeted(m)) return true;
			}
			return false;
		}

		/// <summary>
		/// соответствие дерева метке
		/// </summary>
		/// <param name="marks"></param>
		/// <returns></returns>
		protected bool treelabelin(params string[] marks)
		{
			var current = q.Row.Native;
			if (null == current) return false;
			while (null != current)
			{
				foreach (var m in marks)
				{
					if (current.IsMarkSeted(m)) return true;
				}
				current = current.Parent;
			}
			return false;
		}
		/// <summary>
		/// соответсвие строки тагу
		/// </summary>
		/// <param name="tag"></param>
		/// <returns></returns>
		protected bool rowtagin(params string[] tag)
		{
			if (null == tag) return false;
			foreach (var s in tag)
			{
				var test = TagHelper.Parse(s);
				foreach (var testt in test)
				{
					var testval = testt.Value.ToLower();
					var currentval = TagHelper.Value(q.Row.Tag, testt.Key).ToLower();
					if (currentval.noContent()) return false;
					if (testval == "*") return true;
					if (testval != currentval) return false;
				}
			}
			return true;
		}
		/// <summary>
		/// соответствие дерева тагу
		/// </summary>
		/// <param name="tag"></param>
		/// <returns></returns>
		protected bool treetagin(params string[] tag)
		{
			if (null == tag) return false;
			if (q.Row.Native == null)
			{
				return rowtagin(tag);
			}
			foreach (var s in tag)
			{
				var test = TagHelper.Parse(s);
				foreach (var testt in test)
				{
					var testval = testt.Value.ToLower();
					var currentval = q.Row.Native.ResolveTag(testt.Key).ToLower();
					if (currentval.noContent()) return false;
					if (testval == "*") return true;
					if (testval != currentval) return false;
				}
			}
			return true;
		}
		/// <summary>
		/// соответствие чего угодно на уровне данной строки тексту
		/// </summary>
		/// <param name="txt"></param>
		/// <returns></returns>
		protected bool rowallin(params string[] txt)
		{
			if (rowlabelin(txt)) return true;
			if (rowgroupin(txt)) return true;
			if (rowtagin(txt)) return true;
			return false;
		}
		/// <summary>
		/// соответствие чего угодно в дереве переданному тексту
		/// </summary>
		/// <param name="txt"></param>
		/// <returns></returns>
		protected bool treeallin(params string[] txt)
		{
			if (treelabelin(txt)) return true;
			if (treegroupin(txt)) return true;
			if (treetagin(txt)) return true;
			return false;
		}
		




		/// <summary>
		/// Проверяет соответствие основного объекта набору
		/// </summary>
		/// <param name="objids"></param>
		/// <returns></returns>
		protected bool objin(params int[] objids)
		{ //#ZC-131
			return null != objids && q.Obj.IsForObj && -1 != Array.IndexOf(objids, q.Obj.Id);
		}

		/// <summary>
		/// Проверяет соответствие любого тега набору
		/// </summary>
		/// <param name="tag"></param>
		/// <returns></returns>
		protected bool objtagin(string tag)
		{ //#ZC-131
			if (string.IsNullOrWhiteSpace(tag) || q.Obj.IsNotForObj) return false;
			var objtags = TagHelper.Parse(q.Obj.Tag);
			if (0 == objtags.Count) return false;
			foreach (var test in TagHelper.Parse(tag))
			{
				if (!objtags.ContainsKey(test.Key)) continue;
				if (test.Value == "*" || objtags[test.Key] == test.Value) return true;
			}
			return false;
		}

		/// <summary>
		/// Проверяет соответствие основного объекта всему набору тегов
		/// </summary>
		/// <param name="tag"></param>
		/// <returns></returns>
		protected bool allobjtagin(string tag)
		{ //#ZC-131
			if (string.IsNullOrWhiteSpace(tag) || q.Obj.IsNotForObj) return false;
			var objtags = TagHelper.Parse(q.Obj.Tag);
			if (0 == objtags.Count) return false;
			foreach (var test in TagHelper.Parse(tag))
			{
				if (!objtags.ContainsKey(test.Key)) return false;
				if (!(test.Value == "*" || objtags[test.Key] == test.Value)) return false;
			}
			return true;
		}

		/// <summary>
		/// Проверяет соответствие основного объекта любой группе
		/// </summary>
		/// <param name="codes"></param>
		/// <returns></returns>
		protected bool grpin(params string[] codes)
		{ //#ZC-131
			return null != codes && q.Obj.IsForObj && codes.Any(x => q.Obj.ObjRef.GroupCache.Contains("/" + x + "/"));
		}
		/// <summary>
		/// Проверяет соответствие основного объекта всем группам
		/// </summary>
		/// <param name="codes"></param>
		/// <returns></returns>
		protected bool allgrpin(params string[] codes)
		{ //#ZC-131
			return null != codes && q.Obj.IsForObj && codes.All(x => q.Obj.ObjRef.GroupCache.Contains("/" + x + "/"));
		}
		/// <summary>
		/// Проверяет соответствие основного объекта любой группе
		/// </summary>
		/// <param name="codes"></param>
		/// <returns></returns>
		protected bool divin(params string[] codes)
		{ //#ZC-131
			return null != codes && q.Obj.IsForObj && codes.Any(x => q.Obj.ObjRef.Group.Code == x);
		}
	}
}