#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : SessionAttachedFormulaBase.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme {
	/// <summary>
	/// 	������� ���������� ������ � ��������� ������
	/// </summary>
	public abstract class SessionAttachedFormulaBase : IFormula {
		/// <summary>
		/// 	����������� ������� �� ���������� ���������� ������
		/// </summary>
		/// <param name="query"> </param>
		public void Init(ZexQuery query) {
			Query = query;
			Mastersesion = Query.Session;
			if (null == Mastersesion) {
				Mastersesion = new ZexSession();
				Session = Mastersesion.AsSerial();
			}
			else {
				IsSubSession = true;
				Session = Mastersesion.GetSubSession();
			}
		}


		/// <summary>
		/// 	������� ���������� ����������
		/// </summary>
		/// <returns> </returns>
		/// <remarks>
		/// 	� �������� ����� ���������� ���������� ������� �� ������ ������ �����
		/// </remarks>
		public abstract QueryResult Eval();

		/// <summary>
		/// 	��������� ������� �������� ������� ����� �������������
		/// </summary>
		public void CleanUp() {
			if (IsSubSession) {
				Mastersesion.Return(Session);
			}
		}

		/// <summary>
		/// 	������� ������ � ���-������
		/// </summary>
		protected internal bool IsSubSession;

		/// <summary>
		/// 	������� ������
		/// </summary>
		protected internal ZexSession Mastersesion;

		/// <summary>
		/// 	�������� ������
		/// </summary>
		protected internal ZexQuery Query;

		/// <summary>
		/// 	������� ������
		/// </summary>
		protected internal ISerialSession Session;
	}
}