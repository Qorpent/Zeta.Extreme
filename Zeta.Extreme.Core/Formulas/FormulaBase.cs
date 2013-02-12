using System;

namespace Zeta.Extreme {
	/// <summary>
	/// ������� ����������� �������
	/// </summary>
	public abstract class FormulaBase : IFormula,IDisposable {
		/// <summary>
		/// 	����������� ������� �� ���������� ���������� ������
		/// </summary>
		/// <param name="query"> </param>
		public virtual void Init(Query query) {
			Query = query;
			Session = query.Session;
		}
		/// <summary>
		/// ������ �� �������� �������� �������
		/// </summary>
		protected internal Query Query;

		/// <summary>
		/// ������ �� �������� ������ � ��������������� �������
		/// </summary>
		protected FormulaRequest Descriptor;
		/// <summary>
		/// ���� ���������� � ������ ��������
		/// </summary>
		protected bool IsInPlaybackMode;

		/// <summary>
		/// 	������� ������
		/// </summary>
		protected internal Session Session;

		/// <summary>
		/// ������������� �������� ������������� �������
		/// </summary>
		/// <param name="request"></param>
		public virtual void SetContext(FormulaRequest request) {
			Descriptor = request;
		}

		/// <summary>
		/// ���������� � ���� ����������, ��������� ����� �������, �� ��� ���������� ��������
		/// </summary>
		/// <param name="query"> </param>
		public void Playback(Query query) {
			IsInPlaybackMode = true;
			Init(query);
			InternalEval();
			CleanUp();
		}


		/// <summary>
		/// 	������� ���������� ����������
		/// </summary>
		/// <returns> </returns>
		/// <remarks>
		/// 	� �������� ����� ���������� ���������� ������� �� ������ ������ �����
		/// </remarks>
		public QueryResult Eval() {
			IsInPlaybackMode = false;
			return InternalEval();
		}
		/// <summary>
		/// ��������� ���������� ���������� ����������
		/// </summary>
		/// <returns></returns>
		protected abstract QueryResult InternalEval();

		/// <summary>
		/// 	��������� ������� �������� ������� ����� �������������
		/// </summary>
		public virtual void CleanUp() {
			Query = null;
			Session = null;
		}

		/// <summary>
		/// ��������� ������������ ����������� ������, ��������� � �������������� ��� ������� ������������� ��������.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public void Dispose() {
			CleanUp();
		}
	}
}