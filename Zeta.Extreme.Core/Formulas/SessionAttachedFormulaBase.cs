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
		public void Init(Query query) {
			Query = query;
			Mastersession = Query.Session;
			//if (null == Mastersession) {
			//	Mastersession = new ZexSession();
			//	Session = Mastersession.AsSerial();
			//}
			//else {
			//	IsSubSession = true;
			//	Session = Mastersession.GetSubSession();
			//}
		}

		/// <summary>
		/// ���������� � ���� ����������, ��������� ����� �������, �� ��� ���������� ��������
		/// </summary>
		/// <param name="query"> </param>
		public abstract void Playback(Query query);


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
				Mastersession.Return(Session);
			}
		}

		/// <summary>
		/// 	������� ������ � ���-������
		/// </summary>
		protected internal bool IsSubSession;

		/// <summary>
		/// 	������� ������
		/// </summary>
		protected internal Session Mastersession;

		/// <summary>
		/// 	�������� ������
		/// </summary>
		protected internal Query Query;

		/// <summary>
		/// 	������� ������
		/// </summary>
		protected internal ISerialSession Session;
	}
}