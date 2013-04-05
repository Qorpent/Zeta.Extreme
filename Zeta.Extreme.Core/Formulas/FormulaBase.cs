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
// PROJECT ORIGIN: Zeta.Extreme.Core/FormulaBase.cs
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
			try {
				IsInPlaybackMode = true;
				Init(query);
				InternalEval();
				CleanUp();
			}
			catch (Exception e) {
				query.Result = new QueryResult
					{
						IsComplete = false,
						Error = new QueryException(query,e)
					};
			}
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