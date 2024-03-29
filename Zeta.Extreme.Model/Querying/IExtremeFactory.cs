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
// PROJECT ORIGIN: Zeta.Extreme.Model/IExtremeFactory.cs
#endregion
namespace Zeta.Extreme.Model.Querying {
	/// <summary>
	/// ����������� ������� �������� � ������
	/// </summary>
	public interface IExtremeFactory {
		/// <summary>
		/// ������� ������
		/// </summary>
		/// <returns></returns>
		ISession CreateSession(SessionSetupInfo setupInfo = null);
		/// <summary>
		/// ������� �������
		/// </summary>
		/// <returns></returns>
		IQuery CreateQuery(QuerySetupInfo setupInfo = null);

		/// <summary>
		/// ������� RowHandler
		/// </summary>
		/// <returns></returns>
		IRowHandler CreateRowHandler();
		/// <summary>
		/// ������� ColumnHandler
		/// </summary>
		/// <returns></returns>
		IColumnHandler CreateColumnHandler();

		/// <summary>
		/// ������� ObjHandler
		/// </summary>
		/// <returns></returns>
		IObjHandler CreateObjHandler();


		/// <summary>
		/// ������� TimeHandler
		/// </summary>
		/// <returns></returns>
		ITimeHandler CreateTimeHandler();
		/// <summary>
		/// �������� � ��������� ������
		/// </summary>
		/// <returns></returns>
		IFormulaStorage GetFormulaStorage();
		/// <summary>
		/// ������� ��������� ������
		/// </summary>
		/// <returns></returns>
		IReferenceHandler CreateReference();
	}
}