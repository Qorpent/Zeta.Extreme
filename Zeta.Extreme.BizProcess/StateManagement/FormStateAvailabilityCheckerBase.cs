using System;
using Qorpent;

namespace Zeta.Extreme.BizProcess.StateManagement {
	/// <summary>
	/// Базовый класс расширений анализа возможности установки статуса
	/// </summary>
	public abstract class FormStateAvailabilityCheckerBase : ServiceBase,IFormStateAvailabilityChecker {
		/// <summary>
		/// User-defined order index
		/// </summary>
		public int Index { get; set; }


		/// <summary>
		/// Позволяет установить тип возвращаемого сообщения об ошибке
		/// </summary>
		public FormStateOperationDenyReasonType ReasonType { get; set; }

		/// <summary>
		/// Сообщение об ошибке
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		/// Ошибка при исключении
		/// </summary>
		protected Exception Error { get; set; }

		/// <summary>
		/// Шоткат генерации результата при ошибке
		/// </summary>
		/// <returns></returns>
		protected FormStateOperationResult GetResult() {
			return new FormStateOperationResult{Allow = false, Reason = {Type=ReasonType,Message = Message,Error = Error}};
		}

		/// <summary>
		/// Укажите в случае, если расширение предназначено для проверки только конкретного статуса
		/// </summary>
		public FormStateType TargetFormState { get; set; }

		/// <summary>
		/// Укажите в случае, если расширение предназначено для проверки только конкретного входного статуса
		/// </summary>
		public FormStateType SourceFormState { get; set; }

		/// <summary>
		///     Проверка возможности установки статуса
		/// </summary>
		/// <param name="stateValidationContext"></param>
		/// <returns>
		/// </returns>
		public FormStateOperationResult GetCanSet(StateValidationContext stateValidationContext) {
			Context = stateValidationContext;
			if (null != Component) {
				var basename = Component.Name.Replace(".state.checker", "");
				var ignoreparameter = basename + "_ignore";
				if (Context.Options.ContainsKey(ignoreparameter) && Context.Options[ignoreparameter] == "1") return null;
			}
			if (FormStateType.None != TargetFormState) {
				if (Context.NewState != TargetFormState) return null;
			}
			if (FormStateType.None != SourceFormState)
			{
				if (Context.OldState != SourceFormState) return null;
			}

			if (InternalIsValid()) {
				return null;
			}
			return GetResult();
		}

		/// <summary>
		/// Проверяет соответствие контекста
		/// </summary>
		/// <returns></returns>
		protected abstract bool InternalIsValid();


		/// <summary>
		/// Текущий контекст обработки
		/// </summary>
		protected StateValidationContext Context { get; set; }
	}
}