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
// PROJECT ORIGIN: Zeta.Extreme.BizProcess/IInputTemplate.cs
#endregion
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.XPath;
using Qorpent.Model;
using Zeta.Extreme.BizProcess.Forms;
using Zeta.Extreme.BizProcess.StateManagement;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.BizProcess.Themas {
	/// <summary>
	/// 	��������� ����� �����
	/// </summary>
	public interface IUserForm : IWithName, IWithCode, IWithRole, IMvcBasedInputTemplate {
		/// <summary>
		/// 	�������� ������
		/// </summary>
		RowDescriptor Form { get; set; }

		/// <summary>
		/// 	������
		/// </summary>
		IList<ColumnDesc> Values { get; }

		/// <summary>
		/// 	���������
		/// </summary>
		IDictionary<string, string> Parameters { get; set; }

		/// <summary>
		/// 	��������� �������������� ���������� (�����������)
		/// </summary>
		IList<IUserForm> Sources { get; }

		/// <summary>
		/// 	�������������� ���������
		/// </summary>
		string AdvDocs { get; set; }

		/// <summary>
		/// 	��������������� ��������
		/// </summary>
		string PeriodRedirect { get; set; }

		/// <summary>
		/// 	���������� ������������ �����
		/// </summary>
		string NeedFiles { get; set; }

		/// <summary>
		/// 	���������� � �������� ������������� ������
		/// </summary>
		string NeedFilesPeriods { get; set; }

		/// <summary>
		/// 	������ �� �������� ������������
		/// </summary>
		XPathNavigator SourceXmlConfiguration { get; set; }

		//NOTE: ������ XPathNavigator??
		// IDictionary<string, InputQuery> Queries { get; }
		/// <summary>
		/// 	����������� ���������� ������ (����� ��������)
		/// </summary>
		string Script { get; set; }

		/// <summary>
		/// 	������������� ������� ����������
		/// </summary>
		string CustomSave { get; set; }

		/// <summary>
		/// 	��� ������ ����������
		/// </summary>
		string SaveMethod { get; set; }

		/// <summary>
		/// 	��� ���������� ������
		/// </summary>
		string BindedReport { get; set; }

		/// <summary>
		/// 	��� �������
		/// </summary>
		int Year { get; set; }

		/// <summary>
		/// 	������ �������
		/// </summary>
		int Period { get; set; }

		//NOTE : ��� ��� ������ ��� ��� ��� ���� � ����������� ������������ ���� ���� ������

		/// <summary>
		/// 	������ ���� �������
		/// </summary>
		DateTime DirectDate { get; set; }

		/// <summary>
		/// 	��� �������
		/// </summary>
		string UnderwriteCode { get; set; }

		/// <summary>
		/// 	������ ������
		/// </summary>
		string Help { get; set; }

		///<summary>
		///	������������ ��������������
		///</summary>
		string AutoFillDescription { get; set; }

		/// <summary>
		/// 	������� ���������� �� ������
		/// </summary>
		string ForGroup { get; set; }

		//   IList<InputField> Fields { get; }
		/// <summary>
		/// 	������ �� ��������
		/// </summary>
		int[] ForPeriods { get; set; }

		/// <summary>
		/// 	��� ������� �� ������� (�����)
		/// </summary>
		string DetailFilterName { get; set; }

		/// <summary>
		/// 	������ �� �������
		/// </summary>
		IDetailFilter DetailFilter { get; }

		//NOTE: ���� ����� �����������, ��� �� ����� ������ ������������???

		/// <summary>
		/// 	��������� �� �������
		/// </summary>
		bool DetailSplit { get; set; }
		/*
		/// <summary>
		/// 	������� ����� ��� �������
		/// </summary>
		bool IsForDetail { get; }
		
		/// <summary>
		/// 	������� ����� ��� ����� ������
		/// </summary>
		bool IsForSingleDetail { get; }
		*/
		/// <summary>
		/// 	������������� ������ �����
		/// </summary>
		IList<string> FixedRowCodes { get; set; }
		/*
		/// <summary>
		/// 	������� ����� ��� �������
		/// </summary>
		bool IsInputForDetail { get; }
		*/
		/// <summary>
		/// 	������� ���������� �����
		/// </summary>
		bool IsOpen { get; set; }

		/// <summary>
		/// 	���� �� �������
		/// </summary>
		string UnderwriteRole { get; set; }

		/// <summary>
		/// 	�������� ����������
		/// </summary>
		int ScheduleDelta { get; set; }

		/// <summary>
		/// 	����� ���������� �����
		/// </summary>
		string ScheduleClass { get; set; }

		/// <summary>
		/// 	������� ������������� ������ ��������� �����
		/// </summary>
		bool FavoriteRowsOnly { get; set; }

		/// <summary>
		/// 	����� ������� ����� ��� �������, ����� �����-��
		/// </summary>
		bool InputForDetail { get; set; }

		/// <summary>
		/// 	�������� ������ �� ����
		/// </summary>
		IThema Thema { get; set; }

		/// <summary>
		/// 	������ �����
		/// </summary>
		IList<RowDescriptor> Rows { get; }

		/// <summary>
		/// 	������� ������ ������� � �������� ���������
		/// </summary>
		bool ShowMeasureColumn { get; set; }

		/// <summary>
		/// 	������� ����, ��� ����� ���������
		/// </summary>
		bool IsChecked { get; set; }

		/// <summary>
		/// 	������ ����� �� ���������
		/// </summary>
		string DefaultState { get; set; }

		/// <summary>
		/// 	������� ����������� �������??
		/// </summary>
		bool DetailFavorite { get; set; }

		/// <summary>
		/// 	�������� SQL ����������� ������ �����
		/// </summary>
		string SqlOptimization { get; set; }

		///// <summary>
		///// 	������ �� ������������
		///// </summary>
		//InputConfiguration Configuration { get; set; }

		///<summary>
		///	������ ���������
		///</summary>
		string DocumentRoot { get; set; }

		/// <summary>
		/// 	���������
		/// </summary>
		IDictionary<string, string> Documents { get; set; }

		/// <summary>
		/// 	������������� ��� �������
		/// </summary>
		string FixedObjectCode { get; set; }

		/// <summary>
		/// 	������������� ������
		/// </summary>
		IZetaMainObject FixedObject { get; }

		/// <summary>
		/// 	��������� ������ ������������
		/// </summary>
		bool NeedPreloadScript { get; set; }

		/// <summary>
		/// 	������������ ������� ����������
		/// </summary>
		bool UseQuickUpdate { get; set; }

		/// <summary>
		/// 	������������ ������ �������
		/// </summary>
		bool IgnorePeriodState { get; set; }

		/// <summary>
		/// 	����������� �� �������
		/// </summary>
		bool IsObjectDependent { get; set; }

		/// <summary>
		/// 	������������ �� ���
		/// </summary>
		bool IsActualOnYear { get; }

		/// <summary>
		/// 	������������� ��������
		/// </summary>
		bool UseBizTranMatrix { get; set; }

		/// <summary>
		/// 	����� ��������
		/// </summary>
		string Biztran { get; set; }

		/// <summary>
		/// 	������ ������������ �����
		/// </summary>
		IFormSession AttachedSession { get; set; }

	

		/// <summary>
		/// 	����������� � �������
		/// </summary>
		/// <param name="year"> </param>
		/// <param name="period"> </param>
		/// <param name="directDate"> </param>
		/// <param name="obj"> </param>
		/// <returns> </returns>
		IUserForm PrepareForPeriod(int year, int period, DateTime directDate, IZetaMainObject obj);

		/// <summary>
		/// 	��������� ������������ �������
		/// </summary>
		/// <returns> </returns>
		bool GetIsPeriodMatched();

		/// <summary>
		/// 	�����������
		/// </summary>
		/// <returns> </returns>
		IUserForm Clone();

		//IEnumerable<IZetaCell> GetCellsByTargets(Controller controller);
		// �� �����������

		/// <summary>
		/// 	Gets the state (��������� ������� ������ ������� ����� �� ����������
		/// </summary>
		/// <param name="obj"> The obj. </param>
		/// <param name="detail"> </param>
		/// <returns> </returns>
		string GetState(IZetaMainObject obj, IZetaDetailObject detail);

		/// <summary>
		/// 	���������� ������
		/// </summary>
		/// <param name="obj"> </param>
		/// <param name="detail"> </param>
		/// <param name="state"> </param>
		/// <returns> </returns>
		int SetState(IZetaMainObject obj, IZetaDetailObject detail, string state);

		/// <summary>
		/// 	�������� ������
		/// </summary>
		void RefreshState();

		

		

		/// <summary>
		/// 	�������� ������ ���������
		/// </summary>
		/// <returns> </returns>
		bool GetIsVisible();

		/// <summary>
		/// 	�������� ������ �� ����������
		/// </summary>
		/// <returns> </returns>
		ScheduleState GetScheduleState();

		/// <summary>
		/// 	��������� ����������� ��������� �������
		/// </summary>
		/// <param name="obj"> </param>
		/// <param name="detail"> </param>
		/// <param name="state"> </param>
		/// <returns> </returns>
		string CanSetState(IZetaMainObject obj, IZetaDetailObject detail, string state);

		/// <summary>
		/// 	SQL ���
		/// </summary>
		/// <param name="obj"> </param>
		/// <param name="year"> </param>
		/// <param name="period"> </param>
		/// <returns> </returns>
		IDictionary<string, object> ReloadSqlCache(IZetaMainObject obj, int year, int period);

		/// <summary>
		/// 	�������� ��� �������
		/// </summary>
		/// <returns> </returns>
		IEnumerable<ColumnDesc> GetAllColumns();

		/// <summary>
		/// 	�������� ���������� �������
		/// </summary>
		/// <returns> </returns>
		bool IsPeriodOpen();

		/// <summary>
		/// 	�������� ������
		/// </summary>
		/// <param name="zetaMainObject"> </param>
		/// <param name="detail"> </param>
		/// <param name="statecache"> </param>
		/// <returns> </returns>
		string GetState(IZetaMainObject zetaMainObject, IZetaDetailObject detail, IDictionary<string, object> statecache);

		/// <summary>
		/// 	��������� ������������ ������� �����
		/// </summary>
		/// <param name="obj"> </param>
		/// <returns> </returns>
		bool IsMatch(IZetaMainObject obj);

		/// <summary>
		/// 	���������� ������
		/// </summary>
		/// <param name="obj"> </param>
		/// <param name="detail"> </param>
		/// <param name="state"> </param>
		/// <param name="skipcheck"> </param>
		/// <param name="parent"> </param>
		/// <returns> </returns>
		int SetState(IZetaMainObject obj, IZetaDetailObject detail, string state, bool skipcheck = false, int parent = 0);
		/*
		/// <summary>
		/// 	�������� �������� �����
		/// </summary>
		/// <param name="row"> </param>
		/// <param name="obj"> </param>
		/// <returns> </returns>
		ColumnRowCheckCondition[] GetRowChecks(IZetaRow row, IZetaMainObject obj);

		/// <summary>
		/// 	��������� �������� �����
		/// </summary>
		/// <param name="row"> </param>
		/// <param name="obj"> </param>
		/// <param name="col"> </param>
		/// <returns> </returns>
		ColumnRowCheckCondition[] GetRowChecks(IZetaRow row, IZetaMainObject obj, ColumnDesc col = null);

		/// <summary>
		/// 	�������� ����� ����������� ������
		/// </summary>
		/// <param name="row"> </param>
		/// <param name="obj"> </param>
		/// <returns> </returns>
		string GetCheckedRowClass(IZetaRow row, IZetaMainObject obj);

		/// <summary>
		/// 	�������� ����� ����������� ������
		/// </summary>
		/// <param name="row"> </param>
		/// <param name="obj"> </param>
		/// <returns> </returns>
		string GetCheckedRowStyle(IZetaRow row, IZetaMainObject obj);

		/// <summary>
		/// 	�������� ����� ����������� ������
		/// </summary>
		/// <param name="row"> </param>
		/// <param name="obj"> </param>
		/// <param name="col"> </param>
		/// <returns> </returns>
		string GetCheckedCellClass(IZetaRow row, IZetaMainObject obj, ColumnDesc col);

		/// <summary>
		/// 	�������� ����� ����������� ������
		/// </summary>
		/// <param name="row"> </param>
		/// <param name="obj"> </param>
		/// <param name="col"> </param>
		/// <returns> </returns>
		string GetCheckedCellStyle(IZetaRow row, IZetaMainObject obj, ColumnDesc col);
		*/
		//IList<IFile> GetAttachedFiles(int objid, AttachedFileType filestype, int year = 0, int period = 0);
		// ����� ����������
		/// <summary>
		/// 	�������� �������
		/// </summary>
		void CleanupStates();

		/// <summary>
		/// 	�������� ����������� ������ �����
		/// </summary>
		/// <param name="row"> </param>
		/// <returns> </returns>
		bool IsValidRow(IZetaRow row);
		/*
		/// <summary>
		/// 	�������� ����������� ������
		/// </summary>
		/// <param name="obj"> </param>
		/// <returns> </returns>
		ControlPointResult[] GetControlPoints(IZetaMainObject obj);
		*/
		/// <summary>
		/// 	�������� ������ �������
		/// </summary>
		/// <returns> </returns>
		string GetColGroup();

		/// <summary>
		/// 	�������� ������� �������
		/// </summary>
		/// <param name="obj"> </param>
		/// <returns> </returns>
		List<IZetaMainObject> GetWorkingObjects(IZetaMainObject obj);

		/// <summary>
		/// 	���������� ��������
		/// </summary>
		/// <param name="name"> </param>
		/// <returns> </returns>
		object ResolveParameter(string name);

		/// <summary>
		/// 	��������� ������
		/// </summary>
		/// <param name="obj"> </param>
		/// <param name="values"> </param>
		/// <returns> </returns>
		IList<ColumnDesc> AccomodateColumnSet(IZetaMainObject obj, IList<ColumnDesc> values);

		/// <summary>
		/// 	����������� ������ ���� ��������
		/// </summary>
		/// <param name="obj"> </param>
		/// <returns> </returns>
		Task StartCanSetAsync(IZetaMainObject obj);
	}
}