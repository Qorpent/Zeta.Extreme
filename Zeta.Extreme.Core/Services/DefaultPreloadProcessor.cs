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
	public class DefaultPreloadProcessor : IPreloadProcessor {
		/// <summary>
		/// 	����������� �� �������� � �������� � ������
		/// </summary>
		/// <param name="session"> </param>
		/// <exception cref="NotImplementedException"></exception>
		public DefaultPreloadProcessor(Session session) {
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

			if (null != internalquery.Row.Native && internalquery.Row.IsFormula && !_sumh.IsSum(internalquery.Row.Native)) {
				FormulaStorage.Default.Register(new FormulaRequest
					{
						Key = internalquery.Row.Code,
						Formula = internalquery.Row.Formula,
						Language = internalquery.Row.FormulaType,
						Tags = internalquery.Row.Tag,
						Marks = internalquery.Row.Native.MarkCache
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