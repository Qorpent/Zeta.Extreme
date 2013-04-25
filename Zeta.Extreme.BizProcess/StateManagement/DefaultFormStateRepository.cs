using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Principal;
using Qorpent;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.BizProcess.Forms;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.SqlSupport;

namespace Zeta.Extreme.BizProcess.StateManagement {
	/// <summary>
	/// Простой репозиторий запроса форм и статусов
	/// </summary>
	public class DefaultFormStateRepository : ServiceBase,IFormStateRepository {
		/// <summary>
		///     Возвращает хранимый объект формы
		/// </summary>
		/// <param name="form"></param>
		/// <returns>
		/// </returns>
		public Form GetFormRecord(IFormSession form) {
			var condition = string.Format("Year = {0} and Period = {1} and LockCode = '{2}' and Object = {3}", form.Year,
			                              form.Period, form.Template.UnderwriteCode, form.Object.Id);
			return new NativeZetaReader().ReadForms(condition).FirstOrDefault();
		}

		/// <summary>
		///     Возвращает историю статусов для формы
		/// </summary>
		/// <param name="form"></param>
		/// <returns>
		/// </returns>
		public FormState[] GetFormStateHistory(IFormSession form)
		{
			var condition = string.Format("Year = {0} and Period = {1} and LockCode = '{2}' and Object = {3} order by Version desc", form.Year,
			                              form.Period, form.Template.UnderwriteCode, form.Object.Id);
			return new NativeZetaReader().ReadFormStates(condition).ToArray();
		}

		/// <summary>
		///     Возвращает последнюю запись о смене статуса
		/// </summary>
		/// <param name="form"></param>
		/// <returns>
		/// </returns>
		public FormState GetLastFormState(IFormSession form)
		{
			var condition = string.Format("Year = {0} and Period = {1} and LockCode = '{2}' and Object = {3} order by Version desc", form.Year,
			                              form.Period, form.Template.UnderwriteCode, form.Object.Id);
			return new NativeZetaReader().ReadFormStates(condition).FirstOrDefault();
		}

		/// <summary>
		/// Строки приведения статусов к хранимым значениям
		/// </summary>
		public static readonly string[] StateStrings = new[] {"NONE","0ISOPEN", "0ISBLOCK", "0ISCHECKED", "0ISACCEPTED", "RESERVED1", "RESERVED2", "RESERVED3"};

		/// <summary>
		///     Устанавливает новый статус для формы
		/// </summary>
		/// <param name="form"></param>
		/// <param name="stateType"></param>
		/// <param name="parentId"></param>
		/// <param name="principal">Учетная запись пользователя</param>
		/// <param name="comment"></param>
		public FormState SetState(IFormSession  form, FormStateType stateType, string comment, int parentId, IPrincipal principal = null) {
			principal = principal ?? Application.Principal.CurrentUser;
			int staterecordId = 0;
			using (var c = GetConnection())
			{
				c.WellOpen();
				staterecordId = c.ExecuteScalar<int>(
					@"exec usm.set_state 
                        @obj=@obj,
                        @year=@year,
                        @period=@period,
                        @template=@template,
                        @templatecode=@templatecode,
                        @state=@state,
                        @comment=@comment,
                        @usr=@usr,
                        @parent=@parent",
					new Dictionary<string, object>
						{
							{"@obj", form.Object.Id},
							{"@year", form.Year},
							{"@period", form.Period},
							{"@template", form.Template.UnderwriteCode},
							{"@templatecode", form.Template.Code},
							{"@state", StateStrings[(int)stateType]},
							{"@comment", comment},
							{"@parent", parentId},
							{"@usr", principal.Identity.Name},
						});
			}
			return new NativeZetaReader().ReadFormStates("Id = " + staterecordId).First();
		}

		private IDbConnection GetConnection() {
			return Application.DatabaseConnections.GetConnection("Default");
		}
	}
}