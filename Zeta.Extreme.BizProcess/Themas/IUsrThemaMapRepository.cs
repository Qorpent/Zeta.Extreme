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
// PROJECT ORIGIN: Zeta.Extreme.BizProcess/IUsrThemaMapRepository.cs
#endregion
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.BizProcess.Themas {
	/// <summary>
	/// 	Интерфейс получения ответственных
	/// </summary>
	public interface IUsrThemaMapRepository {
		/// <summary>
		/// 	Первый ответственный
		/// </summary>
		/// <param name="thema"> </param>
		/// <param name="system"> </param>
		/// <param name="obj"> </param>
		/// <returns> </returns>
		IUsrThemaMap GetResponsibility(string thema, string system, IZetaMainObject obj);

		/// <summary>
		/// 	Второй ответственный
		/// </summary>
		/// <param name="thema"> </param>
		/// <param name="system"> </param>
		/// <param name="obj"> </param>
		/// <returns> </returns>
		IUsrThemaMap GetResponsibility2(string thema, string system, IZetaMainObject obj);

		/// <summary>
		/// 	Установить ответсвенноо
		/// </summary>
		/// <param name="thema"> </param>
		/// <param name="system"> </param>
		/// <param name="obj"> </param>
		/// <param name="usr"> </param>
		void SetResponsibility(string thema, string system, IZetaMainObject obj, IZetaUnderwriter usr);

		/// <summary>
		/// 	Установить второго ответственного
		/// </summary>
		/// <param name="thema"> </param>
		/// <param name="system"> </param>
		/// <param name="obj"> </param>
		/// <param name="usr"> </param>
		void SetResponsibility2(string thema, string system, IZetaMainObject obj, IZetaUnderwriter usr);
	}
}