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
// PROJECT ORIGIN: Zeta.Extreme.BizProcess/AttachedFileType.cs
#endregion
using System;

namespace Zeta.Extreme.BizProcess.Forms {
	/// <summary>
	/// 	Тип присоединенного файла
	/// </summary>
	[Flags]
	public enum AttachedFileType {
		/// <summary>
		/// 	Никакой
		/// </summary>
		None = 0,

		/// <summary>
		/// 	По умолчанию
		/// </summary>
		Default = 1,

		/// <summary>
		/// 	Дополнительный
		/// </summary>
		Advanced = 2,

		/// <summary>
		/// 	Ссылочный
		/// </summary>
		Correlated = 4,

		/// <summary>
		/// 	Все
		/// </summary>
		All = 8,

		/// <summary>
		/// 	Связанный
		/// </summary>
		Related = 16,
	}
}