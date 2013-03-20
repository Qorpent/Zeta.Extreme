#region LICENSE
// Copyright 2007-2013 Qorpent Team - http://github.com/Qorpent
// Supported by Media Technology LTD 
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//  
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// PROJECT ORIGIN: Zeta.Extreme.FrontEnd/SaveDataAction.cs
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
			_xmldata = Application.Container.Get<ISpecialXmlParser>("json.xml.parser").Parse(_jsonSaveData);
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