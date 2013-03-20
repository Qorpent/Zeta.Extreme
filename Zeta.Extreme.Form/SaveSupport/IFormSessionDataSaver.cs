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
// PROJECT ORIGIN: Zeta.Extreme.Form/IFormSessionDataSaver.cs
#endregion
using System;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Xml.Linq;
using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.Form.SaveSupport {
	/// <summary>
	/// 	Интерфейс класса для сохранения данных форм
	/// </summary>
	public interface IFormSessionDataSaver {
		/// <summary>
		/// 	Текущая стадия процесса сохранения
		/// </summary>
		SaveStage Stage { get; set; }

		/// <summary>
		/// 	Последняя возникшая ошибка
		/// </summary>
		Exception Error { get; set; }

		/// <summary>
		/// 	Метод
		/// </summary>
		/// <param name="session"> </param>
		/// <param name="savedata"> </param>
		/// <param name="currentUser"> </param>
		/// <returns> </returns>
		/// <exception cref="NotImplementedException"></exception>
		Task<SaveResult> BeginSave(IFormSession session, XElement savedata, IPrincipal currentUser);
	}
}