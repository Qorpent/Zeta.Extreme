using System;
using System.Linq;
using Qorpent;
using Zeta.Extreme.BizProcess.StateManagement.Attributes;

namespace Zeta.Extreme.BizProcess.StateManagement {
	/// <summary>
	/// Базовый класс расширений анализа возможности установки статуса
	/// </summary>
	public abstract class FormStateAvailabilityCheckerBase : ServiceBase,IFormStateAvailabilityChecker {
		/// <summary>
		/// Данный конструктор настраивает объект исходя из примененных атрибутов
		/// </summary>
		protected FormStateAvailabilityCheckerBase() {
			var attributes = GetType().GetCustomAttributes(typeof (StateAttributeBase), true).OfType<StateAttributeBase>().ToArray();
			foreach (var attribute in attributes) {
				var errorMessage = attribute as ErrorMessageAttribute;
				if (null != errorMessage) {
					Message = errorMessage.Message;
					continue;
				}
				var targetState = attribute as TargetStateAttribute;
				if (null != targetState)
				{
					TargetFormState = targetState.Type;
					continue;
				}
				var sourceState = attribute as SourceStateAttribute;
				if (null != sourceState)
				{
					SourceFormState = sourceState.Type;
					continue;
				}
				var checkIndex = attribute as CheckIndexAttribute;
				if (null != checkIndex)
				{
					Index = checkIndex.Index;
					continue;
				}

				var reasonType = attribute as ReasonTypeAttribute;
				if (null != reasonType)
				{
					ReasonType = reasonType.Type;
					continue;
				}
				var skipRole = attribute as SkipRoleAttribute;
				if (null != skipRole)
				{
					SkipRole = skipRole.Role;
					SkipRoleExact = skipRole.Exact;
					continue;
				}

				var defaultAllow = attribute as DefaultResultAttribte;
				if (null != defaultAllow) {
					UseDefaultResult = true;
					DefaultResult = defaultAllow.Allow;
					continue;

				}
			}

		}
		/// <summary>
		/// Результат по умолчанию
		/// </summary>
		protected bool DefaultResult { get; set; }

		/// <summary>
		/// Признак использования результата по умолчанию
		/// </summary>
		protected bool UseDefaultResult { get; set; }

		/// <summary>
		/// Признак того что <see cref="SkipRole"/> должна быть явно привязана к пользователю
		/// </summary>
		protected bool SkipRoleExact { get; set; }

		/// <summary>
		/// Системная роль, игнорирующая данную проверку
		/// </summary>
		public string SkipRole { get; set; }

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
			return new FormStateOperationResult{Allow = false, Reason = new FormStateOperationDenyReason {Type=ReasonType,Message = Message,Error = Error}};
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
			if (!string.IsNullOrWhiteSpace(SkipRole)) {
				var isInSkipRole = Context.RoleResolver.IsInRole(Context.Form.Usr, SkipRole, SkipRoleExact);
				if (isInSkipRole) {
					return null;
				}
			}
			var allow = UseDefaultResult ? DefaultResult : InternalIsValid();
			if (allow) {
				return null;
			}
			return GetResult();
		}

		/// <summary>
		/// Проверяет соответствие контекста
		/// </summary>
		/// <returns></returns>
		protected virtual bool InternalIsValid() {
			return false;
		}


		/// <summary>
		/// Текущий контекст обработки
		/// </summary>
		protected StateValidationContext Context { get; set; }
	}
}