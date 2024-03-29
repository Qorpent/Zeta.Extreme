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
// PROJECT ORIGIN: Zeta.Extreme.Core/BackwardCompatibleFormulaBase.cs
#endregion
using System;
using System.Linq;
using System.Text.RegularExpressions;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Model.Extensions;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme {
	/// <summary>
	/// 	������� ������� ��� ������ ������������� � BOO ��������� ����������� ���������
	/// </summary>
	public abstract class BackwardCompatibleFormulaBase : DeltaFormulaBase {
		private static readonly int[] months = new[] {11, 12, 13, 14, 15, 16, 17, 18, 19, 110, 111, 112};
		private static readonly int[] aggs = new[] {22, 1, 24, 25, 2, 27, 28, 3, 210, 211, 4};
		private static readonly int[] plans = new[] {301, 303, 306, 309, 251, 252, 253, 254};
		private static readonly int[] ozhids = new[] {401, 403, 406, 409};

		private static readonly int[] korrektives = new[]
			{31, 32, 33, 34, 311, 312, 313, 314, 321, 322, 323, 324, 331, 332, 333, 334, 341, 342, 343, 344};

		/// <summary>
		/// 	����������� �� ��������� - ���������� ����� ������ ��������
		/// </summary>
		protected BackwardCompatibleFormulaBase() {
			f = new BackwardCompatibleMainFormulaSet(this);
		}

		/// <summary>
		/// 	������ ��� ������������� �� ������� ���������
		/// </summary>
		protected IQuery q {
			get { return Query; }
		}

		/// <summary>
		/// 	������ ��� ������������� �� ������� ���������
		/// </summary>
		protected IQuery query {
			get { return Query; }
		}


		/// <summary>
		/// 	������� �������� � ���� �������
		/// </summary>
		public int year {
			get { return q.Time.Year; }
		}

		/// <summary>
		/// 	��������� �� ������ � �������
		/// </summary>
		protected bool ismonth {
			get { return Array.IndexOf(months, q.Time.Period) != -1; }
		}

		/// <summary>
		/// 	��������� �� ������ � ���������
		/// </summary>
		protected int monthInKvart {
			get { return f.monthInKvart(q.Time.Period); }
		}

		/// <summary>
		/// 	��������� �� ������ � �����������
		/// </summary>
		protected bool iskorrperiod {
			get { return Array.IndexOf(korrektives, q.Time.Period) != -1; }
		}

		/// <summary>
		/// 	��������� �� ������ � ��������
		/// </summary>
		protected bool issumperiod {
			get { return Array.IndexOf(aggs, q.Time.Period) != -1; }
		}

		/// <summary>
		/// 	��������� �� ������ � ��������
		/// </summary>
		protected bool isplanperiod {
			get { return Array.IndexOf(plans, q.Time.Period) != -1; }
		}

		/// <summary>
		/// 	��������� �� ������ � ���������
		/// </summary>
		protected bool isozhidperiod {
			get { return Array.IndexOf(ozhids, q.Time.Period) != -1; }
		}

		/// <summary>
		/// 	��������� ����������� ����� ������� �������
		/// </summary>
		/// <param name="codes"> </param>
		/// <returns> </returns>
		protected bool colin(params string[] codes) {
			return
				codes.Select(x => x.ToUpper()).Any(
					x => x.ToUpper() == q.Col.Code.ToUpper() || (q.Col.Native != null && q.Col.Native.Code.ToUpper() == x.ToUpper()));
			//return Array.IndexOf(codes, q.Column.Code) != -1;
		}

		/// <summary>
		/// 	��������� ������������ ������ �����
		/// </summary>
		/// <param name="codes"> </param>
		/// <returns> </returns>
		protected bool pathin(params string[] codes) {
			if (q.Row.Native == null) {
				return false;
			}
			return codes.Any(x => q.Row.Native.Path.Contains("/" + x + "/"));
		}


		/// <summary>
		/// 	��������� �������������� ������� ������ ������
		/// </summary>
		/// <param name="codes"> </param>
		/// <returns> </returns>
		protected bool rowin(params string[] codes) {
			return Array.IndexOf(codes, q.Row.Code) != -1;
		}

		/// <summary>
		/// 	������ ���������� ���������
		/// </summary>
		/// <param name="name"> </param>
		/// <returns> </returns>
		protected string gets(string name) {
			return getp(name).ToStr();
		}

		/// <summary>
		/// 	������ ��������� ���������
		/// </summary>
		/// <param name="name"> </param>
		/// <returns> </returns>
		protected decimal getn(string name) {
			return getp(name).ToDecimal();
		}

		/// <summary>
		/// 	��������� ������������ ���������
		/// </summary>
		/// <param name="name"> </param>
		/// <returns> </returns>
		protected object getp(string name) {
			return query.ResolveRealCode(name);
		}

		/// <summary>
		/// 	�������� ����� ����
		/// </summary>
		/// <param name="tag"> </param>
		/// <param name="mask"> </param>
		/// <returns> </returns>
		protected bool taglike(string tag, string mask) {
			var tagvalue = q.Row.Native.ResolveTag(tag);
			return Regex.IsMatch(tagvalue, mask);
		}

		/// <summary>
		/// 	�������� ������ ����
		/// </summary>
		/// <param name="tag"> </param>
		/// <param name="mask"> </param>
		/// <returns> </returns>
		protected bool tagstart(string tag, string mask) {
			return taglike(tag, "^" + mask);
		}

		/// <summary>
		/// 	�������� ����� ����
		/// </summary>
		/// <param name="tag"> </param>
		/// <param name="mask"> </param>
		/// <returns> </returns>
		protected bool tagend(string tag, string mask) {
			return taglike(tag, mask + "$");
		}

		/// <summary>
		/// 	�������� ���� ������� ������
		/// </summary>
		/// <param name="tag"> </param>
		/// <param name="mask"> </param>
		/// <returns> </returns>
		protected bool tag(string tag, string mask) {
			return taglike(tag, "^" + mask + "$");
		}

		/// <summary>
		/// 	�� ������������� �� ������ ������ ����� �������� ������ ����������� ��� �������
		/// </summary>
		/// <param name="groups"> </param>
		/// <returns> </returns>
		protected bool groupin(params string[] groups) {
			if (query.Obj.IsForObj) {
				var objgroups = query.Obj.ObjRef.GroupCache.SmartSplit(false, true, '/');
				return groups.Intersect(objgroups).Any();
			}
			throw new NotSupportedException("�� ������ ������ ��������� ���� ����� � Zeta.Extreme ���������");

			/*
			foreach (var g in groups)
			{
				if (q.Zone.GroupFilter == g) return true; // ��� ������ �� �������
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
		/// 	��������� ����������� �������� ������� ������
		/// </summary>
		/// <param name="periods"> </param>
		/// <returns> </returns>
		protected bool periodin(params int[] periods) {
			return Array.IndexOf(periods, q.Time.Period) != -1;
		}

		/// <summary>
		/// 	��������� �� ����� ����� �������� ������� ������
		/// </summary>
		/// <param name="contostring"> </param>
		/// <returns> </returns>
		protected bool contoin(string contostring) {
			if (IsInPlaybackMode) {
				return false; //prevent ambigous queries
			}
			throw new NotSupportedException("�� ������ ������ ��������� ������� � ������ � Zeta.Extreme ���������");
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


		/// <summary>
		/// 	������������ ������ ������
		/// </summary>
		/// <param name="groups"> </param>
		/// <returns> </returns>
		protected bool rowgroupin(params string[] groups) {
			if (null == q.Row.Native) {
				return false;
			}
			var grps = ((IZetaFormsSupport) q.Row.Native).GroupCache.SmartSplit(false, true, '/', ';');
			if (0 == grps.Count) {
				return false;
			}
			return groups.Any(g => grps.Any(x => x == g));
		}

		/// <summary>
		/// 	������������ ������ ������
		/// </summary>
		/// <param name="groups"> </param>
		/// <returns> </returns>
		protected bool treegroupin(params string[] groups) {
			var current = q.Row.Native;
			if (null == current) {
				return false;
			}

			while (null != current) {
				var grps = ((IZetaFormsSupport) current).GroupCache.SmartSplit(false, true, '/', ';');
				if (groups.Any(g => grps.Any(x => x == g))) {
					return true;
				}
				current = current.Parent;
			}
			return false;
		}

		/// <summary>
		/// 	������������ ������ �����
		/// </summary>
		/// <param name="marks"> </param>
		/// <returns> </returns>
		protected bool rowlabelin(params string[] marks) {
			if (null == q.Row.Native) {
				return false;
			}
			foreach (var m in marks) {
				if (q.Row.Native.IsMarkSeted(m)) {
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// 	������������ ������ �����
		/// </summary>
		/// <param name="marks"> </param>
		/// <returns> </returns>
		protected bool treelabelin(params string[] marks) {
			var current = q.Row.Native;
			if (null == current) {
				return false;
			}
			while (null != current) {
				if (marks.Any(m => current.IsMarkSeted(m))) {
					return true;
				}
				current = current.Parent;
			}
			return false;
		}

		/// <summary>
		/// 	����������� ������ ����
		/// </summary>
		/// <param name="tag"> </param>
		/// <returns> </returns>
		protected bool rowtagin(params string[] tag) {
			if (null == tag) {
				return false;
			}
			foreach (var s in tag) {
				var test = TagHelper.Parse(s);
				foreach (var testt in test) {
					var testval = testt.Value.ToLower();
					var currentval = TagHelper.Value(q.Row.Tag, testt.Key).ToLower();
					if (currentval.IsEmpty()) {
						return false;
					}
					if (testval == "*") {
						return true;
					}
					if (testval != currentval) {
						return false;
					}
				}
			}
			return true;
		}

		/// <summary>
		/// 	������������ ������ ����
		/// </summary>
		/// <param name="tag"> </param>
		/// <returns> </returns>
		protected bool treetagin(params string[] tag) {
			if (null == tag) {
				return false;
			}
			if (q.Row.Native == null) {
				return rowtagin(tag);
			}
			foreach (var s in tag) {
				var test = TagHelper.Parse(s);
				foreach (var testt in test) {
					var testval = testt.Value.ToLower();
					var currentval = q.Row.Native.ResolveTag(testt.Key).ToLower();
					if (currentval.IsEmpty()) {
						return false;
					}
					if (testval == "*") {
						return true;
					}
					if (testval != currentval) {
						return false;
					}
				}
			}
			return true;
		}

		/// <summary>
		/// 	������������ ���� ������ �� ������ ������ ������ ������
		/// </summary>
		/// <param name="txt"> </param>
		/// <returns> </returns>
		protected bool rowallin(params string[] txt) {
			if (rowlabelin(txt)) {
				return true;
			}
			if (rowgroupin(txt)) {
				return true;
			}
			if (rowtagin(txt)) {
				return true;
			}
			return false;
		}

		/// <summary>
		/// 	������������ ���� ������ � ������ ����������� ������
		/// </summary>
		/// <param name="txt"> </param>
		/// <returns> </returns>
		protected bool treeallin(params string[] txt) {
			if (treelabelin(txt)) {
				return true;
			}
			if (treegroupin(txt)) {
				return true;
			}
			if (treetagin(txt)) {
				return true;
			}
			return false;
		}


		/// <summary>
		/// 	��������� ������������ ��������� ������� ������
		/// </summary>
		/// <param name="objids"> </param>
		/// <returns> </returns>
		protected bool objin(params int[] objids) {
			//#ZC-131
			return null != objids && q.Obj.IsForObj && -1 != Array.IndexOf(objids, q.Obj.Id);
		}

		/// <summary>
		/// 	��������� ������������ ������ ���� ������
		/// </summary>
		/// <param name="tag"> </param>
		/// <returns> </returns>
		protected bool objtagin(string tag) {
			//#ZC-131
			if (string.IsNullOrWhiteSpace(tag) || q.Obj.IsNotForObj) {
				return false;
			}
			var objtags = TagHelper.Parse(q.Obj.Tag);
			if (0 == objtags.Count) {
				return false;
			}
			foreach (var test in TagHelper.Parse(tag)) {
				if (!objtags.ContainsKey(test.Key)) {
					continue;
				}
				if (test.Value == "*" || objtags[test.Key] == test.Value) {
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// 	��������� ������������ ��������� ������� ����� ������ �����
		/// </summary>
		/// <param name="tag"> </param>
		/// <returns> </returns>
		protected bool allobjtagin(string tag) {
			//#ZC-131
			if (string.IsNullOrWhiteSpace(tag) || q.Obj.IsNotForObj) {
				return false;
			}
			var objtags = TagHelper.Parse(q.Obj.Tag);
			if (0 == objtags.Count) {
				return false;
			}
			foreach (var test in TagHelper.Parse(tag)) {
				if (!objtags.ContainsKey(test.Key)) {
					return false;
				}
				if (!(test.Value == "*" || objtags[test.Key] == test.Value)) {
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// 	��������� ������������ ��������� ������� ����� ������
		/// </summary>
		/// <param name="codes"> </param>
		/// <returns> </returns>
		protected bool grpin(params string[] codes) {
			//#ZC-131
			return null != codes && q.Obj.IsForObj && codes.Any(x => q.Obj.ObjRef.GroupCache.Contains("/" + x + "/"));
		}

		/// <summary>
		/// 	��������� ������������ ��������� ������� ���� �������
		/// </summary>
		/// <param name="codes"> </param>
		/// <returns> </returns>
		protected bool allgrpin(params string[] codes) {
			//#ZC-131
			return null != codes && q.Obj.IsForObj && codes.All(x => q.Obj.ObjRef.GroupCache.Contains("/" + x + "/"));
		}

		/// <summary>
		/// 	��������� ������������ ��������� ������� ����� ������
		/// </summary>
		/// <param name="codes"> </param>
		/// <returns> </returns>
		protected bool divin(params string[] codes) {
			//#ZC-131
			return null != codes && q.Obj.IsForObj && codes.Any(x => q.Obj.ObjRef.Division.Code == x);
		}

		/// <summary>
		/// 	�������� � ������������ ������������ �����������
		/// </summary>
		protected BackwardCompatibleMainFormulaSet f;
	}
}