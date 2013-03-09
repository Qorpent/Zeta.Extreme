#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : HtmlListDefinition.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;
using System.Text;
using Qorpent.Utils.Extensions;

namespace Zeta.Extreme.Form.Themas {
	/// <summary>
	/// 	��������� ������ HTML ��� ��������
	/// </summary>
	public class HtmlListDefinition {
		/// <summary>
		/// 	�� �������
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// 	��������
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 	��������
		/// </summary>
		public string Value { get; set; }

		/// <summary>
		/// 	����� (?)
		/// </summary>
		public string Class { get; set; }

		/// <summary>
		/// 	����� ���������
		/// </summary>
		public string Text {
			get { return _text.IsNotEmpty() ? _text : Value; }
			set { _text = value; }
		}

		/// <summary>
		/// 	������� ������
		/// </summary>
		public bool IsGroup {
			get { return Items.Count > 0; }
		}

		/// <summary>
		/// </summary>
		/// <returns> </returns>
		public override string ToString() {
			return Render();
		}

		/// <summary>
		/// 	��������� ������� � ������
		/// </summary>
		/// <param name="value"> </param>
		/// <param name="text"> </param>
		/// <returns> </returns>
		public HtmlListDefinition Add(string value, string text) {
			var result = new HtmlListDefinition {Value = value, Text = text};
			Items.Add(result);
			return result;
		}

		/// <summary>
		/// 	������������ ������� ������� �� HTML
		/// </summary>
		/// <returns> </returns>
		public string Render() {
			if (Items.Count == 0) {
				return "<span class='empty'>������� ������� �����������</span>";
			}
			var result = new StringBuilder();
			Render(result, true);
			return result.ToString();
		}

		/// <summary>
		/// 	��������� ���������
		/// </summary>
		/// <param name="builder"> </param>
		/// <param name="root"> </param>
		protected void Render(StringBuilder builder, bool root) {
			if (root) {
				builder.AppendFormat("<select id='{0}' name='{1}' class='{2}'>", Id, Name, Class);
			}
			if (!root && IsGroup) {
				builder.AppendFormat("<optgroup label='{0}'>", Text);
			}

			if (IsGroup) {
				foreach (var item in Items) {
					item.Render(builder, false);
				}
			}
			else {
				builder.AppendFormat("<option value='{0}'>{1}</option>", Value, Text);
			}
			if (root) {
				builder.Append("</select>");
			}
			if (!root && IsGroup) {
				builder.Append("</optgroup>");
			}
		}

		/// <summary>
		/// 	�������� ��� ���������
		/// </summary>
		public readonly IList<HtmlListDefinition> Items = new List<HtmlListDefinition>();

		private string _text;
	}
}