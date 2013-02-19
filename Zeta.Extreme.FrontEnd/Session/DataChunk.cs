using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qorpent.Serialization;

namespace Zeta.Extreme.FrontEnd.Session
{
	/// <summary>
	/// Описывает возвращаемый набор данных из сессии
	/// оптимизирован по трафику и для сериализации в JSON
	/// </summary>
	[Serialize]
	public class DataChunk {
		/// <summary>
		/// Первый индекс
		/// </summary>
		[SerializeNotNullOnly]
		public int si;
		/// <summary>
		/// Последний переданный индекс
		/// </summary>
		[SerializeNotNullOnly]
		public int ei;
		/// <summary>
		///Статус передачи (e,w,f) 
		/// </summary>
		[Serialize]
		public string state;
		/// <summary>
		/// Сообщение об ошибке
		/// </summary>
		[SerializeNotNullOnly]
		public string e;
		/// <summary>
		/// Собственно возвращаемые данные
		/// </summary>
		[SerializeNotNullOnly]
		public OutCell[] data;
	}
}
