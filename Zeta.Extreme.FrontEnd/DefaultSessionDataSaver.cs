using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;
using Zeta.Extreme.Form;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// Сервис сохранения данных формы по умолчанию
	/// </summary>
	public class DefaultSessionDataSaver : IFormSessionDataSaver {
		/// <summary>
		/// Метод 
		/// </summary>
		/// <param name="session"></param>
		/// <param name="savedata"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public Task<SaveResult> BeginSave(IFormSession session, XElement savedata) {
			if(null!=Current && !Current.IsCompleted) {
				Current.Wait();
			}
			Current = null;
			Stage = SaveStage.Load;
			Error = null;
			return (Current = Task.Run(() => InternalSave(session,savedata)));
		}
		/// <summary>
		/// Собственно процедура сохранения, вызывается всегда асинхронно
		/// </summary>
		/// <param name="session"></param>
		/// <param name="savedata"></param>
		/// <returns></returns>
		protected virtual SaveResult InternalSave(IFormSession session, XElement savedata) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// Текущая стадия процесса сохранения
		/// </summary>
		public SaveStage Stage { get; set; }
		/// <summary>
		/// Последняя возникшая ошибка
		/// </summary>
		public Exception Error { get; set; }
		/// <summary>
		/// Текущая задача
		/// </summary>
		public Task<SaveResult> Current { get; set; }
	}
}