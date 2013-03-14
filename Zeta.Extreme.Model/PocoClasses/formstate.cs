#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : formstate.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Qorpent.Serialization;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Model.PocoClasses {
	[Serialize]
	public class FormState : IFormState {
		[Serialize] public virtual string ReadableState {
			get { return GetReadableState(); }
		}

		/// <summary>
		/// 	Идентификатор формы
		/// </summary>
		[IgnoreSerialize] public int FormId { get; set; }

		/// <summary>
		/// 	Идентификатор родительского статуса
		/// </summary>
		[SerializeNotNullOnly] public int ParentId { get; set; }

		[IgnoreSerialize] public virtual IForm Form { get; set; }

		public virtual string State { get; set; }

		public virtual string Usr { get; set; }

		[SerializeNotNullOnly] public virtual IFormState Parent { get; set; }

		public virtual string GetReadableState() {
			switch (State) {
				case "0ISOPEN":
					return "Открыта";
				case "0ISBLOCK":
					return "Закрыта";
				case "0ISCHECKED":
					return "Проверена";
			}
			return State;
		}

		public virtual int Id { get; set; }

		public virtual DateTime Version { get; set; }
		[SerializeNotNullOnly] public virtual string Comment { get; set; }
	}
}