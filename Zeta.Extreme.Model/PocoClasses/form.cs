#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : form.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Model {
	public class Form : IForm {
		public virtual string TemplateCode { get; set; }

		/// <summary>
		/// 	Идентификатор объекта
		/// </summary>
		public virtual int ObjectId { get; set; }

		public virtual int Year { get; set; }

		public virtual int Period { get; set; }

		public virtual string Template { get; set; }

		public virtual IList<IFormState> States { get; set; }

		public virtual string CurrentState { get; set; }

		public virtual int Id { get; set; }


		public virtual string Code { get; set; }


		public virtual DateTime Version { get; set; }


		public virtual IZetaMainObject Object { get; set; }


		public virtual IFormState GetLastState() {
			var result = States != null ? States.OrderBy(x => x.Version).LastOrDefault() : null;
			if (null == result) {
				result = new FormState();
				result.Form = this;
				result.State = "0ISOPEN";
				result.Usr = "система";
			}
			return result;
		}

		public virtual IFormState GetLastBlock() {
			var result = States != null ? States.Where(x => x.State == "0ISBLOCK").OrderBy(x => x.Version).LastOrDefault() : null;
			if (null == result) {
				result = new FormState();
				result.Form = this;
				result.State = "0ISOPEN";
				result.Usr = "система";
			}
			return result;
		}
	}
}