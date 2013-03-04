#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : TypedParameter.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Reflection;
using Comdiv.Extensions;

namespace Zeta.Extreme.BizProcess.Themas {
	/// <summary>
	/// 	Типизированный параметр темы
	/// </summary>
	public class TypedParameter {
		/// <summary>
		/// 	Конструктор по умолчанию
		/// </summary>
		public TypedParameter() {
			Type = typeof (Missing);
		}

		/// <summary>
		/// 	Расчитать итоговое значение
		/// </summary>
		/// <returns> </returns>
		public object GetValue() {
			if (Value != null && Type == typeof (Missing)) {
				Type = typeof (string);
			}
			return Value.to(Type);
		}

		/// <summary>
		/// 	Порядковый номер параметра
		/// </summary>
		public int Idx;

		/// <summary>
		/// 	Режим параметра
		/// </summary>
		public string Mode; //NOTE: что еще за режим?

		/// <summary>
		/// 	Имя параметра
		/// </summary>
		public string Name;

		/// <summary>
		/// 	Тип параметра
		/// </summary>
		public Type Type;

		/// <summary>
		/// 	Значение параметра
		/// </summary>
		public string Value;
	}
}