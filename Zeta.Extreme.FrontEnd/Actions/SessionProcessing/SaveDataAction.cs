#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : SaveDataAction.cs
// Project: Zeta.Extreme.FrontEnd
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Xml.Linq;
using Qorpent.Dsl;
using Qorpent.Mvc;
using Qorpent.Mvc.Binding;

namespace Zeta.Extreme.FrontEnd.Actions.SessionProcessing {
	///<summary>
	///	Вызывает сохранение данных
	///</summary>
	[Action("zefs.save")]
	public class SaveDataAction : FormSessionActionBase {
		/// <summary>
		/// 	Third part of execution - setup system-bound internal state here (called after validate, but before authorize)
		/// </summary>
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
		/// 	Параметр для данных для сохранения
		/// </summary>
		[Bind(Name = "data", Required = true)] private string _jsonSaveData = "";

		private XElement _xmldata;
	}
}