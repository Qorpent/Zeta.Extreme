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
// PROJECT ORIGIN: Zeta.Extreme.Form/InputConfiguration.cs
#endregion
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.BizProcess.Themas;
using Zeta.Extreme.Form.InputTemplates;

namespace Zeta.Extreme.Form.Themas {
	/// <summary>
	/// 	��������� ������������ ����� �����
	/// </summary>
	public class InputConfiguration : ItemConfigurationBase<IInputTemplate> {
		/// <summary>
		/// 	������� ����������� �� ��������
		/// </summary>
		public bool IsObjectDependent { get; set; }

		/// <summary>
		/// 	���� ����������
		/// </summary>
		public string[] Sources { get; set; }

		/// <summary>
		/// 	�������� ����������
		/// </summary>
		public string Lock { get; set; }

		/// <summary>
		/// 	�������� �������� � ��������
		/// </summary>
		public string ForPeriods { get; set; }

		/// <summary>
		/// 	�������� ��������� ��������������
		/// </summary>
		public string AutoFill { get; set; }

		/// <summary>
		/// 	����� ���������� ����������
		/// </summary>
		public string ScheduleClass { get; set; }

		/// <summary>
		/// 	������� ���������� ����� �����������
		/// </summary>
		public string ForGroup { get; set; }

		/// <summary>
		/// 	������������� ������
		/// </summary>
		public string FixRows { get; set; }

		/// <summary>
		/// 	���������� ���������� ������� ����� ���������
		/// </summary>
		public bool NeedPreloadScript { get; set; }

		/// <summary>
		/// 	������ ����� �� ���������
		/// </summary>
		public string DefaultState { get; set; }

		/// <summary>
		/// 	�������� ����������
		/// </summary>
		public int ScheduleDelta { get; set; }

		/// <summary>
		/// 	��� �������
		/// </summary>
		public string TableView { get; set; }

		/// <summary>
		/// 	��������� ������ �� ������� (������ ��� m140)
		/// </summary>
		public bool DetailFavorite { get; set; }

		/// <summary>
		/// 	���������� ������� � ���������
		/// </summary>
		public bool ShowMeasureColumn { get; set; }

		/// <summary>
		/// 	������� ���������� ������
		/// </summary>
		public string NeedFilesPeriods { get; set; }

		/// <summary>
		/// 	������� ���������� �������
		/// </summary>
		public string NeedFiles { get; set; }

		/// <summary>
		/// 	�������������� ���������
		/// </summary>
		public string AdvDocs { get; set; }

		/// <summary>
		/// 	������������� ������
		/// </summary>
		public string FixedObj { get; set; }

		/// <summary>
		/// 	������ �� ����� BIZTRAN
		/// </summary>
		public string Biztran { get; set; }

		/// <summary>
		/// 	����� ��� �������
		/// </summary>
		public bool InputForDetail { get; set; }

		/// <summary>
		/// 	XML - ����������� �������
		/// </summary>
		public XElement[] ColumnDefinitions { get; set; }

		/// <summary>
		/// 	XML - ����������� �����
		/// </summary>
		public XElement[] RowDefinitions { get; set; }

		/// <summary>
		/// 	������� ������������� ������ ��������� �����
		/// </summary>
		public bool FavoriteRowsOnly { get; set; }

		/// <summary>
		/// 	������� ��������
		/// </summary>
		public string PeriodRedirect { get; set; }

		/// <summary>
		/// 	���� �� ����������
		/// </summary>
		public string UnderwriteRole { get; set; }

		/// <summary>
		/// 	�������� ������
		/// </summary>
		public string RootCode { get; set; }

		/// <summary>
		/// 	SQL �����������
		/// </summary>
		public string SqlOptimization { get; set; }


		/// <summary>
		/// 	���������
		/// </summary>
		public IDictionary<string, string> Documents {
			get { return _documents; }
			set { _documents = value; }
		}

		/// <summary>
		/// 	�������� ������
		/// </summary>
		public string DocumentRoot { get; set; }

		/// <summary>
		/// 	������������ ������� ����������
		/// </summary>
		public bool UseQuickUpdate { get; set; }

		/// <summary>
		/// 	������������ ������ ��������
		/// </summary>
		public bool IgnorePeriodState { get; set; }

		/// <summary>
		/// 	������� �� ����������������
		/// </summary>
		/// <returns> </returns>
		public override IInputTemplate Configure() {
			//var txs = new InputTemplateXmlSerializer();
			//var template = txs.Read(TemplateXml).First();
			var template = new InputTemplate();
			template.UnderwriteCode = Lock;
			template.Code = Code;
			template.ForGroup = ForGroup;
			template.Role = Role;
			template.SqlOptimization = SqlOptimization;
			template.PeriodRedirect = PeriodRedirect;
			template.Name = Name;
			template.ForPeriods = ForPeriods.SmartSplit().Select(x => x.ToInt()).ToArray();
			template.AutoFillDescription = AutoFill;
			template.UnderwriteRole = UnderwriteRole;
			template.ScheduleDelta = ScheduleDelta;
			template.Biztran = Biztran;
			template.ScheduleClass = ScheduleClass;
			template.FixedObjectCode = FixedObj;
			template.DefaultState = DefaultState;
			template.DetailFavorite = DetailFavorite;
			template.IgnorePeriodState = IgnorePeriodState;
			template.IsObjectDependent = IsObjectDependent;
			// template.TableView = TableView;
			template.NeedFiles = NeedFiles;
			template.NeedPreloadScript = NeedPreloadScript;
			template.DocumentRoot = DocumentRoot;
			template.NeedFilesPeriods = NeedFilesPeriods;
			template.UseQuickUpdate = UseQuickUpdate;
			template.AdvDocs = AdvDocs;
			if (FixRows.IsNotEmpty()) {
				foreach (var s in FixRows.SmartSplit()) {
					template.FixedRowCodes.Add(s);
				}
			}
			template.FavoriteRowsOnly = FavoriteRowsOnly;
			template.InputForDetail = InputForDetail;
			template.ShowMeasureColumn = ShowMeasureColumn;

			template.Configuration = this;

			if (ColumnDefinitions.ToBool()) {
				var serializer = new InputTemplateXmlSerializer();
				serializer.BindColumns(template, ColumnDefinitions);
			}


			if (RowDefinitions.ToBool()) {
				var serializer = new InputTemplateXmlSerializer();
				serializer.BindRows(template, RowDefinitions);
			}

			if (template.Form == null) {
				var rowcodeparam = RootCode;
				if (rowcodeparam.IsEmpty()) {
					rowcodeparam = Thema.ResolveParameter("rootrow").GetValue() as string;
				}

				if (rowcodeparam.IsNotEmpty()) {
					template.Form = new RowDescriptor {Code = rowcodeparam};
				}
			}

			foreach (var document in Documents) {
				template.Documents[document.Key] = document.Value;
			}

			foreach (var p in Parameters) {
				template.Parameters[p.Name] = p.Value;
			}

			return template;
		}

		private IDictionary<string, string> _documents = new Dictionary<string, string>();
	}
}