#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : DefaultZexPreloadProcessor.cs
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
	public class DefaultZexPreloadProcessor : IZexPreloadProcessor {
		/// <summary>
		/// 	����������� �� �������� � �������� � ������
		/// </summary>
		/// <param name="zexSession"> </param>
		/// <exception cref="NotImplementedException"></exception>
		public DefaultZexPreloadProcessor(ZexSession zexSession) {
			_session = zexSession;
			_sumh = new ZetaVirtualSumHelper();
		}

		/// <summary>
		/// 	��������� �������������
		/// </summary>
		/// <param name="query"> </param>
		/// <returns> </returns>
		public virtual ZexQuery Process(ZexQuery query) {
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

			if (null != internalquery.Row.Native && internalquery.Row.IsFormula && !_sumh.IsSum(internalquery.Row.Native)) {
				FormulaStorage.Default.Register(new FormulaRequest
					{
						Key = internalquery.Row.Formula.Trim(),
						Formula = internalquery.Row.Formula,
						Language = internalquery.Row.FormulaType,
						Tags = internalquery.Row.Tag,
						Marks = internalquery.Row.Native.MarkCache
					});
			}

			return internalquery;
		}


		private readonly ZetaVirtualSumHelper _sumh;

		/// <summary>
		/// 	�������� ������ �� ������
		/// </summary>
		protected ZexSession _session;
	}
}