#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : FormElement.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Comdiv.Extensions;

namespace Zeta.Extreme.Form.SaveSupport {
	/// <summary>
	/// </summary>
	public class FormElement {
		/// <summary>
		/// </summary>
		public FormElement() {
			Attributes = new Dictionary<string, string>();
			ElementName = GetType().Name;
		}

		//identity block
		/// <summary>
		/// </summary>
		public string Code { get; set; }

		/// <summary>
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// </summary>
		public string Title { get; set; }


		//html block
		/// <summary>
		/// </summary>
		public IList<string> CssClasses { get; set; }

		/// <summary>
		/// </summary>
		public string CssClass {
			get { return CssClasses.Distinct().concat(" "); }
		}

		/// <summary>
		/// </summary>
		public IDictionary<string, string> Attributes { get; set; }

		/// <summary>
		/// </summary>
		public string Text { get; set; }


		//xml generation/parsing
		/// <summary>
		/// </summary>
		public string ElementName { get; set; }

		/// <summary>
		/// </summary>
		public object Tag { get; set; }

		/// <summary>
		/// </summary>
		public int Idx { get; set; }

		/// <summary>
		/// </summary>
		/// <returns> </returns>
		public XElement ToXml() {
			var e = new XElement(ElementName);
			e
				.setattr("code", Code)
				.setattr("name", Name)
				.setattr("title", Title)
				.setattr("class", CssClass)
				;
			foreach (var attribute in Attributes) {
				e.setattr(attribute.Key, attribute.Value);
			}

			innerToXml(e);

			e.Value = Text;

			return e;
		}

		/// <summary>
		/// </summary>
		/// <param name="e"> </param>
		protected virtual void innerToXml(XElement e) {}

		/// <summary>
		/// </summary>
		/// <param name="e"> </param>
		public void FromXml(XElement e) {
			ElementName = e.Name.LocalName;
			Code = e.attr("code");
			Name = e.attr("name");
			Title = e.attr("title");
			CssClasses = e.attr("class").split(false, true, ' ').Distinct().ToList();
			Text = e.Value;
			foreach (var attribute in e.Attributes()) {
				if (attribute.Name.isIn("code", "name", "title", "class")) {
					continue;
				}
				Attributes[attribute.Name.LocalName] = attribute.Value;
			}
			innerFromXml(e);
		}

		/// <summary>
		/// 	/
		/// </summary>
		/// <param name="e"> </param>
		protected virtual void innerFromXml(XElement e) {}
	}
}