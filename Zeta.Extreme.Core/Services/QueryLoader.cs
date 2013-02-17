#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : QueryLoader.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Comdiv.Extensions;

namespace Zeta.Extreme {
	/// <summary>
	/// 	������������ �� �������� �� ���������
	/// </summary>
	public class QueryLoader : IPreloadProcessor {
		/// <summary>
		/// 	����������� �� �������� � �������� � ������
		/// </summary>
		/// <param name="session"> </param>
		/// <exception cref="NotImplementedException"></exception>
		public QueryLoader(Session session) {
			_session = session;
			_sumh = new StrongSumProvider();
		}

		/// <summary>
		/// 	��������� �������������
		/// </summary>
		/// <param name="query"> </param>
		/// <returns> </returns>
		public virtual Query Process(Query query) {
			var internalquery = query.Copy(true);
			// ������ ������ �������� ������ � �������
			// ��� ����� ����������������� ��������� ������������� �� ���� ����������

			//������� �������� ����������� ��������� ������������ �������

			internalquery.Normalize(_session);
			if (internalquery.Row.Native != null && internalquery.Row.Native.IsMarkSeted("0CAPTION")) {
				return null; //it's not processable query
			}
			var obsolete = TagHelper.Value(internalquery.Row.Tag, "obsolete").toInt();
			if (obsolete != 0) {
				if (obsolete <= internalquery.Time.Year) {
					return null;
				}
			}

			if (internalquery.Row.IsFormula && !_sumh.IsSum(query.Row)) {
				FormulaStorage.Default.Register(new FormulaRequest
					{
						Key = "row:"+ internalquery.Row.Code,
						Formula = internalquery.Row.Formula,
						Language = internalquery.Row.FormulaType,
						Tags = internalquery.Row.Tag,
						Marks = internalquery.Row.Native==null?"": internalquery.Row.Native.MarkCache
					});
			}

			if (internalquery.Col.IsFormula && !_sumh.IsSum(query.Col)) {
				var key = "col:" + internalquery.Col.Code;
				if(null==internalquery.Col.Native) {
					key = "dyncol:" + internalquery.Col.Formula;
				}
				FormulaStorage.Default.Register(new FormulaRequest
				{
					Key =key,
					Formula = internalquery.Col.Formula,
					Language = internalquery.Col.FormulaType,
					Tags = internalquery.Col.Tag,
					Marks = internalquery.Col.Native == null ? "" : internalquery.Col.Native.MarkCache
				});
			}

			if (internalquery.Obj.IsFormula && !_sumh.IsSum(query.Obj))
			{
				FormulaStorage.Default.Register(new FormulaRequest
				{
					Key = "obj:" + internalquery.Obj.Code,
					Formula = internalquery.Obj.Formula,
					Language = internalquery.Obj.FormulaType,
					Tags = internalquery.Obj.Tag
				});
			}


			return internalquery;
		}


		private readonly StrongSumProvider _sumh;

		/// <summary>
		/// 	�������� ������ �� ������
		/// </summary>
		protected Session _session;
	}
}