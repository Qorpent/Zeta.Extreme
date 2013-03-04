#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : DataChunk.cs
// Project: Zeta.Extreme.FrontEnd
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Serialization;
using Zeta.Extreme.BizProcess.Forms;
using Zeta.Extreme.Form;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// 	Описывает возвращаемый набор данных из сессии
	/// 	оптимизирован по трафику и для сериализации в JSON
	/// </summary>
	[Serialize]
	public class DataChunk {
		/// <summary>
		/// 	Собственно возвращаемые данные
		/// </summary>
		[SerializeNotNullOnly] public OutCell[] data;

		/// <summary>
		/// 	Сообщение об ошибке
		/// </summary>
		[SerializeNotNullOnly] public string e;

		/// <summary>
		/// 	Последний переданный индекс
		/// </summary>
		[SerializeNotNullOnly] public int ei;

		/// <summary>
		/// 	Первый индекс
		/// </summary>
		[SerializeNotNullOnly] public int si;

		///<summary>
		///	Статус передачи (e,w,f)
		///</summary>
		[Serialize] public string state;
	}
}