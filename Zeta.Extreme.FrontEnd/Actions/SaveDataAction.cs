using System;
using System.Xml.Linq;
using Qorpent.Dsl;
using Qorpent.Mvc;
using Qorpent.Mvc.Binding;
using Qorpent.Serialization;

namespace Zeta.Extreme.FrontEnd.Actions {
	/// <summary>
	///Вызывает сохранение данных
	/// </summary>
	[Action("zefs.save")]
	public class SaveDataAction : FormSessionActionBase
	{
		protected override void Prepare() {
			_xmldata = new JsonToXmlParser().Parse(_jsonSaveData);
		}
		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess() {
			return MySession.BeginSaveData(_xmldata);
		}
		/// <summary>
		/// Параметр для данных для сохранения
		/// </summary>
		[Bind(Name = "data",Required = true)] private string _jsonSaveData = "";

		private XElement _xmldata;
	}
}