using System.Globalization;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Developer.DataMigrations {
	/// <summary>
	/// Промежуточный класс для подготовки скрипта переноса
	/// </summary>
	public class TransferRecord {
		/// <summary>
		/// Код исходной строки
		/// </summary>
		public string SourceRow { get; set; }
		/// <summary>
		/// Целевая строка
		/// </summary>
		public int TargetRowId { get; set; }
		/// <summary>
		/// Код исходной колонки
		/// </summary>
		public string SourceColumn { get; set; }
		/// <summary>
		/// Идентификатор целевой колонки
		/// </summary>
		public int TargetColumnId { get; set; }
		/// <summary>
		/// Исходный год
		/// </summary>
		public int SourceYear { get; set; }
		/// <summary>
		/// Целевой год
		/// </summary>
		public int TargetYear { get; set; }

		/// <summary>
		/// Исходный период
		/// </summary>
		public int SourcePeriod { get; set; }
		/// <summary>
		/// Исходный период
		/// </summary>
		public int TargetPeriod { get; set; }

			

		/// <summary>
		/// Исходный объект
		/// </summary>
		public int SourceObject { get; set; }

		/// <summary>
		/// Целевая валюта
		/// </summary>
		public string TargetCurrency { get; set; }

		/// <summary>
		/// Целевой объект
		/// </summary>
		public int TargetObject { get; set; }

		/// <summary>
		/// Создает запрос для вычисления значения
		/// </summary>
		/// <returns></returns>
		public Query CreateQuery() {
			return new Query(SourceRow, SourceColumn, SourceObject, SourceYear, SourcePeriod){Currency = TargetCurrency};
		}
		/// <summary>
		/// Возвращает
		/// </summary>
		/// <param name="result"></param>
		/// <returns></returns>
		public string CreateUpdateSqlQuery(decimal result) {

			return string.Format(@"insert usm.insertdata (year, period, row, col, obj, decimalvalue, usr, valuta, op) values ({0},{1},{2},{3},{4},{5},'{6}','{7}','=')",
			                     TargetYear, TargetPeriod, TargetRowId, TargetColumnId, TargetObject, 
			                     result.ToString("0.######", CultureInfo.InvariantCulture), UserCode, TargetCurrency);
		}
		/// <summary>
		/// Условный код переноса пользователя
		/// </summary>
		public string UserCode { get; set; }

		/// <summary>
		/// Автоматически рассчитывет недостающие значения
		/// </summary>
		public void AutoSetup() {
			if (0 == TargetYear) TargetYear = SourceYear;
			if (0 == TargetPeriod) TargetPeriod = SourcePeriod;
			if (0 == TargetObject) TargetObject = SourceObject;
			if (0 == TargetRowId) {
				TargetRowId = MetaCache.Default.Get<IZetaRow>(SourceRow).Id;
			}
			if (0 == TargetColumnId)
			{
				TargetColumnId = MetaCache.Default.Get<IZetaColumn>(SourceColumn).Id;
			}
			if (string.IsNullOrWhiteSpace(TargetCurrency)) {
				TargetCurrency = "NONE";
			}
			
		}
	}
}