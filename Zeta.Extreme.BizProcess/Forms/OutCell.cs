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
// PROJECT ORIGIN: Zeta.Extreme.BizProcess/OutCell.cs
#endregion

using System;
using Qorpent.Serialization;
using Zeta.Extreme.Model.Querying;
using Qorpent.Utils.Extensions;
namespace Zeta.Extreme.BizProcess.Forms {
	/// <summary>
	/// Структура, для представления ячейки в форме ввода
	/// </summary>
	public class OutCell {
		/// <summary>
		/// 	Ссылка на Id ячейки в БД
		/// </summary>
		[SerializeNotNullOnly] public int c;

		/// <summary>
		/// 	Признак значения, которое может быть целью сохранения
		/// </summary>
		[IgnoreSerialize] public bool canbefilled;

		/// <summary>
		/// 	Уникальный ИД ячейки
		/// </summary>
		public string i;

		/// <summary>
		/// 	Позволяет связать две ячейки в разных наборах
		/// </summary>
		[IgnoreSerialize] public OutCell linkedcell;

		/// <summary>
		/// 	Значение ячейки
		/// </summary>
		[SerializeNotNullOnly] public string v;

		/// <summary>
		/// Реальный ключ ячейки
		/// </summary>
		[SerializeNotNullOnly]public string ri;

		/// <summary>
		/// 	Ссылка на запрос для заполняемых значений
		/// </summary>
		[IgnoreSerialize] public IQuery query;
		/// <summary>
		/// Признак наличия ошибок
		/// </summary>
		public bool iserror;
		/// <summary>
		/// Ошибка
		/// </summary>
		[IgnoreSerialize]
		public Exception error;

		/// <summary>
		/// Определяет - является ли данная ячейка нулевой (пустая или 0-е значение или -)
		/// </summary>
		public bool IsZero {
			get {
				if (string.IsNullOrWhiteSpace(v)) return true;
				if ("-" == v) return true;
				if (v.ToDecimal() == 0) return true;
				return false;
			}
		}
	}
}