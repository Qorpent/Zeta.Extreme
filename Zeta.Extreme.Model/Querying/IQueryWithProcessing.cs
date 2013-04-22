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
// PROJECT ORIGIN: Zeta.Extreme.Core/IQueryWithProcessing.cs
#endregion

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Model.Querying {
	/// <summary>
	/// 	��������� ������� � ���������� ���������
	/// </summary>
	public interface IQueryWithProcessing : IQuery {
		

		/// <summary>
		/// 	Sign that primary was not set
		/// </summary>
		bool HavePrimary { get; set; }

		/// <summary>
		/// 	Back-reference to preparation tasks
		/// </summary>
		Task PrepareTask { get; set; }

		/// <summary>
		/// ���������, �������� �� ������ �����������
		/// </summary>
		bool GetIsRecycle(IDictionary<long,bool> registry = null );

		/// <summary>
		/// 	Client processed mark
		/// </summary>
		bool Processed { get; set; }

		/// <summary>
		/// 	������ �� ����������
		/// </summary>
		PrepareState PrepareState { get; set; }

		/// <summary>
		/// 	�������� �������
		/// </summary>
		IList<IQuery> FormulaDependency { get; }

		/// <summary>
		/// 	��������� "����������� �������"
		/// </summary>
		bool IsPrimary { get; }

		/// <summary>
		/// 	����������� ��� �������� ��������
		/// </summary>
		IList<Tuple<decimal, IQuery>> SummaDependency { get; }

		/// <summary>
		/// 	�������, ������� �������������� � ������� �� ���� ����������
		/// </summary>
		IFormula AssignedFormula { get; set; }

		/// <summary>
		/// 	��� ���������� �������
		/// </summary>
		QueryEvaluationType EvaluationType { get; set; }
		/// <summary>
		/// ��������� ���� �������
		/// </summary>
		string AssignedFormulaKey { get; set; }

		/// <summary>
		/// 	��������� ���������������� ������� � ����������
		/// </summary>
		/// <param name="timeout"> </param>
		void WaitPrepare(int timeout = -1);
	}
}