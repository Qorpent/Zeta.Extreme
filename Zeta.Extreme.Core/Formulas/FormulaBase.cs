#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : FormulaBase.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.Querying;

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
		public virtual void Init(IQuery query) {
			if (!(query is IQueryWithProcessing)) throw new Exception("cannot process query that are not IQueryWithProcessing");
			Query = query as IQueryWithProcessing;
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
		public void Playback(IQuery query) {
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
		protected internal IQueryWithProcessing Query;

		/// <summary>
		/// 	������� ������
		/// </summary>
		protected internal ISession Session;
	}
}