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
// PROJECT ORIGIN: Zeta.Extreme.BizProcess/IThemaHelper.cs
#endregion
using Zeta.Extreme.BizProcess.Reports;

namespace Zeta.Extreme.BizProcess.Themas {
	/// <summary>
	/// 	Стандартный хелпер для тем (для веба)
	/// </summary>
	public interface IThemaHelper {
		/// <summary>
		/// 	Получить тему
		/// </summary>
		/// <param name="code"> </param>
		/// <returns> </returns>
		IThema get(string code);

		/// <summary>
		/// 	Получить конфигурацию
		/// </summary>
		/// <param name="code"> </param>
		/// <returns> </returns>
		IThemaConfiguration getcfg(string code);

		/// <summary>
		/// 	Получить форму
		/// </summary>
		/// <param name="code"> </param>
		/// <returns> </returns>
		IInputTemplate getform(string code);

		/// <summary>
		/// 	Получить отчет
		/// </summary>
		/// <param name="code"> </param>
		/// <returns> </returns>
		IReportDefinition getreport(string code);

		/// <summary>
		/// 	Получить дочерние
		/// </summary>
		/// <param name="thema"> </param>
		/// <returns> </returns>
		IThema[] getchildren(IThema thema);

		/// <summary>
		/// 	Получить дочерние (по коду)
		/// </summary>
		/// <param name="themacode"> </param>
		/// <returns> </returns>
		IThema[] getchildren(string themacode);

		/// <summary>
		/// 	Получить группы
		/// </summary>
		/// <returns> </returns>
		IThema[] getgroups();

		/// <summary>
		/// 	Получить корневые темы для группы
		/// </summary>
		/// <param name="group"> </param>
		/// <returns> </returns>
		IThema[] getgrouproots(IThema group);

		/// <summary>
		/// 	Получить корневые темы для группы (по коду)
		/// </summary>
		/// <param name="groupcode"> </param>
		/// <returns> </returns>
		IThema[] getgrouproots(string groupcode);

		/// <summary>
		/// 	Проверка, что тема тестовая
		/// </summary>
		/// <param name="thema"> </param>
		/// <returns> </returns>
		bool istest(IThema thema);

		/// <summary>
		/// 	Проверка, что тема в разработке
		/// </summary>
		/// <param name="thema"> </param>
		/// <returns> </returns>
		bool isdesign(IThema thema);

		/// <summary>
		/// 	Проверка, что тема - админская
		/// </summary>
		/// <param name="thema"> </param>
		/// <returns> </returns>
		bool isadmin(IThema thema);
	}
}