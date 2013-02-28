using System;
using Comdiv.Zeta.Model;
using Qorpent.Serialization;

namespace Zeta.Extreme.Poco {
	[Serialize]
	public class formstate : IFormState{
		#region IFormState Members
		[IgnoreSerialize]
		public virtual IForm Form { get; set; }

		public virtual string State { get; set; }

		public virtual string Usr { get; set; }

		[SerializeNotNullOnly]
		public virtual IFormState Parent { get; set; }
		public virtual string GetReadableState() {
			switch (State) {
				case "0ISOPEN":
					return "�������";
				case "0ISBLOCK":
					return "�������";
				case "0ISCHECKED":
					return "���������";
			}
			return State;
		}
		[Serialize]
		public virtual string ReadableState {
			get { return GetReadableState(); }
		}

		public virtual int Id { get; set; }

		public virtual DateTime Version { get; set; }
		[SerializeNotNullOnly]
		public virtual string Comment { get; set; }
		/// <summary>
		/// ������������� �����
		/// </summary>
		[IgnoreSerialize]
		public int FormId { get; set; }
		/// <summary>
		/// ������������� ������������� �������
		/// </summary>
		[SerializeNotNullOnly]
		public int ParentId { get; set; }

		#endregion
	}
}