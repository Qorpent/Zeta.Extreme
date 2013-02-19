using System.Xml.Linq;
using Qorpent.Dsl;
using Qorpent.Mvc;
using Qorpent.Mvc.Binding;

namespace Zeta.Extreme.FrontEnd.Actions {
	/// <summary>
	/// ���������� ������� ������ ����������
	/// </summary>
	[Action("zefs.savestate")]
	public class SaveStateAction : FormSessionActionBase
	{
		protected override void Prepare()
		{
			_xmldata = new JsonToXmlParser().Parse(_jsonSaveData);
		}
		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess() {
			
			return MySession.GetSaveState();
		}
		/// <summary>
		/// �������� ��� ������ ��� ����������
		/// </summary>
		[Bind(Name = "data", Required = true)]
		private string _jsonSaveData = "";

		private XElement _xmldata;
	}
}