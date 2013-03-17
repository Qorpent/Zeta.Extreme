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
// PROJECT ORIGIN: Zeta.Extreme.Form/ControlPointResult.cs
#endregion
using Zeta.Extreme.BizProcess.Themas;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme.Form.StateManagement {
	/// <summary>
	/// 	Результат сверки контрольной точки
	/// </summary>
	public class ControlPointResult {
		/// <summary>
		/// 	Провереная строка
		/// </summary>
		public IZetaRow Row { get; set; }

		/// <summary>
		/// 	Проверенная колонка
		/// </summary>
		public ColumnDesc Col { get; set; }

		/// <summary>
		/// 	Итоговое значение
		/// </summary>
		public decimal Value { get; set; }

		/// <summary>
		/// 	Проверка валидности контрольной точки
		/// </summary>
		public bool IsValid {
			get { return Value == 0; }
		}

		/// <summary>
		/// 	Ссылка на исходный запрос, позволяет отсрочить получение значения
		/// </summary>
		public IQuery Query { get; set; }
	}
}