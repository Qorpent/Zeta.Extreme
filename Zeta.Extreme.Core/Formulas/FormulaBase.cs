#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : FormulaBase.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme {
	/// <summary>
	/// 	������� ����������� �������
	/// </summary>
	public abstract class FormulaBase : IFormula, IDisposable {
		/// <summary>
		/// 	��������� ������������ ����������� ������, ��������� � �������������� ��� ������� ������������� ��������.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public void Dispose() {
			CleanUp();
		}

		/// <summary>
		/// 	����������� ������� �� ���������� ���������� ������
		/// </summary>
		/// <param name="query"> </param>
		public virtual void Init(Query query) {
			Query = query;
			Session = query.Session;
		}

		/// <summary>
		/// 	������������� �������� ������������� �������
		/// </summary>
		/// <param name="request"> </param>
		public virtual void SetContext(FormulaRequest request) {
			Descriptor = request;
		}

		/// <summary>
		/// 	���������� � ���� ����������, ��������� ����� �������, �� ��� ���������� ��������
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
		/// 	��������� ������� �������� ������� ����� �������������
		/// </summary>
		public virtual void CleanUp() {
			Query = null;
			Session = null;
		}

		/// <summary>
		/// 	��������� ���������� ���������� ����������
		/// </summary>
		/// <returns> </returns>
		protected abstract QueryResult InternalEval();

		/// <summary>
		/// 	������ �� �������� ������ � ��������������� �������
		/// </summary>
		protected FormulaRequest Descriptor;

		/// <summary>
		/// 	���� ���������� � ������ ��������
		/// </summary>
		protected bool IsInPlaybackMode;

		/// <summary>
		/// 	������ �� �������� �������� �������
		/// </summary>
		protected internal Query Query;

		/// <summary>
		/// 	������� ������
		/// </summary>
		protected internal ISession Session;
	}
}