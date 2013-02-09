#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : DefaultZexPreloadProcessor.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;

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
			internalquery.Normalize();

			return internalquery;
		}


		/// <summary>
		/// 	�������� ������ �� ������
		/// </summary>
		protected ZexSession _session;
	}
}