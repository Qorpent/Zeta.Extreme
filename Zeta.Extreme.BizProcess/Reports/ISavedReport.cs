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
// PROJECT ORIGIN: Zeta.Extreme.BizProcess/ISavedReport.cs
#endregion
using System.Collections.Generic;
using System.Security.Principal;
using Qorpent.Model;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.BizProcess.Reports{
	/// <summary>
	/// Интерфейс сохраненного отчета
	/// </summary>
    public interface ISavedReport : IEntity, IWithUser{
		/// <summary>
		/// Признак общего отчета
		/// </summary>
        bool Shared { get; set; }
		/// <summary>
		/// Код отчета
		/// </summary>
        string ReportCode { get; set; }
		/// <summary>
		/// Сохраненные параметры
		/// </summary>
        IList<ISavedReportParameter> Parameters { get; set; }
		/// <summary>
		/// Роль доступа к отчету
		/// </summary>
        string Role { get; set; }
		/// <summary>
		/// Авторизация отчета
		/// </summary>
		/// <param name="usr"></param>
		/// <returns></returns>
        bool Authorize(IPrincipal usr);
    }
}