using Qorpent.Utils.Extensions;

namespace Zeta.Extreme.Model
{
	/// <summary>
	/// Константы для интеграции с БД
	/// </summary>
	public	class ZetaClassicConst {
		/// <summary>
		/// Историческая старая схема
		/// </summary>
		public const string OldSchema = "usm";
		/// <summary>
		/// Схема для приватных объектов БД
		/// </summary>
		public const string PrivateSchema = "zetai";
		/// <summary>
		/// Схема для внешних API
		/// </summary>
		public const string PublicSchema = "zeta";

		/// <summary>
		/// Процедура триггера после сохранения формы
		/// </summary>
		public const string Form_AFTER_SAVE_TRIGGER = "after_save_trigger";

		/// <summary>
		/// Запрос на выполнение тригера после сохранения
		/// </summary>
		public static readonly UniSqlQuery AfterSaveTriggerQuery = new UniSqlQuery(
			OldSchema,
			Form_AFTER_SAVE_TRIGGER,
			new
				{
					form = UniSqlConst.External,
					obj = UniSqlConst.External,
					year = UniSqlConst.External,
					period = UniSqlConst.External,
					usr = UniSqlConst.External
				});
	}
}
