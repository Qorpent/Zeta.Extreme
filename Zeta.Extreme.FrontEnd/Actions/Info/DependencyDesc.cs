using Qorpent.Serialization;

namespace Zeta.Extreme.FrontEnd.Actions.Info {
	[Serialize]
	internal class DependencyDesc {
		public string code;
		public string name;
		public string outercode;
		public string formcode;
		public string form;
		public object[] forms;
		public string type;
		public object dependency;
		public string[] values;
	}
}