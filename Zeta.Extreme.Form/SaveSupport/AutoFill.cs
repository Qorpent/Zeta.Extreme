#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : AutoFill.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;
using System.Linq;
using Comdiv.Extensions;
using Comdiv.Zeta.Model;
using Zeta.Extreme.Form.InputTemplates;

namespace Zeta.Extreme.Form.SaveSupport {
	/// <summary>
	/// 	����� ������������ ��������������
	/// </summary>
	public class AutoFill {
		/// <summary>
		/// 	������� ���������������
		/// </summary>
		public AutoFill() {
			Periods = new List<int>();
		}

		/// <summary>
		/// 	��������� ��������������
		/// </summary>
		/// <param name="description"> </param>
		public AutoFill(string description) : this() {
			if (description.hasContent()) {
				var def = description.split(false, true, '|');
				if (1 < def.Count) {
					Periods.addRange(def[0].split().Select(s => s.toInt()));
				}
				CallData = def[def.Count - 1];
				if (CallData.hasContent()) {
					AutoFillType = AutoFillType.Custom;
					if (CallData.StartsWith("sql:")) {
						AutoFillType = AutoFillType.Sql;
						CallData = CallData.Substring(4);
					}
				}
			}
		}

		/// <summary>
		/// 	�������������� �� �������
		/// </summary>
		/// <param name="template"> </param>
		public AutoFill(IInputTemplate template) : this(template.AutoFillDescription) {
			Template = template;
		}

		/// <summary>
		/// 	������� �����
		/// </summary>
		public IInputTemplate Template { get; set; }

		/// <summary>
		/// 	�������
		/// </summary>
		public IList<int> Periods { get; set; }

		/// <summary>
		/// 	��� ��������������
		/// </summary>
		public AutoFillType AutoFillType { get; set; }

		/// <summary>
		/// 	������ ��� ����� ???
		/// </summary>
		public string CallData { get; set; }

		/// <summary>
		/// 	�������� �������������
		/// </summary>
		public bool IsExecutable {
			get {
				if (AutoFillType.None == AutoFillType) {
					return false;
				}
				if (CallData.noContent()) {
					return false;
				}
				if (0 != Periods.Count) {
					if (!Periods.Contains(Template.Period)) {
						return false;
					}
				}
				return true;
			}
		}

		/// <summary>
		/// 	������ ��������������
		/// </summary>
		/// <param name="obj"> </param>
		public void Perform(IZetaMainObject obj) {
			if (IsExecutable) {
				var executor = getExecutor();
				executor.Execute(this, obj);
			}
		}

		private IAutoFillExecutor getExecutor() {
			if (AutoFillType.Sql == AutoFillType) {
				return new SqlAutoFiller();
			}
			else {
				return CallData.toType().create<IAutoFillExecutor>();
			}
		}
	}
}