#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : DataSaverBase.cs
// Project: Zeta.Extreme.FrontEnd
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using System.Xml.Linq;
using Qorpent;

namespace Zeta.Extreme.Form.SaveSupport {
	/// <summary>
	/// 	Базовый абстрактный класс для сохранения данных, паттерн - шаблонный метод
	/// </summary>
	public abstract class DataSaverBase : ServiceBase, IFormSessionDataSaver {
		/// <summary>
		/// 	Текущая задача
		/// </summary>
		public Task<SaveResult> Current { get; set; }

		/// <summary>
		/// 	Последний результат сохранения
		/// </summary>
		public SaveResult LastSaveResult { get; set; }

		/// <summary>
		/// 	Метод
		/// </summary>
		/// <param name="session"> </param>
		/// <param name="savedata"> </param>
		/// <returns> </returns>
		/// <exception cref="NotImplementedException"></exception>
		public Task<SaveResult> BeginSave(IFormSession session, XElement savedata) {
			if (null != Current && !Current.IsCompleted) {
				Current.Wait();
			}
			Current = null;
			Stage = SaveStage.Load;
			Error = null;
			return (Current = Task.Run(() => InternalSave(session, savedata)));
		}

		/// <summary>
		/// 	Текущая стадия процесса сохранения
		/// </summary>
		public SaveStage Stage { get; set; }

		/// <summary>
		/// 	Последняя возникшая ошибка
		/// </summary>
		public Exception Error { get; set; }

		/// <summary>
		/// 	Собственно процедура сохранения, вызывается всегда асинхронно
		/// </summary>
		/// <param name="session"> </param>
		/// <param name="savedata"> </param>
		/// <returns> </returns>
		protected SaveResult InternalSave(IFormSession session, XElement savedata) {
			var result = new SaveResult();
			try {
				Stage = SaveStage.Authorize;
				Authorize(session, savedata, result);
				Stage = SaveStage.Prepare;
				Prepare(session, savedata, result);
				Stage = SaveStage.Validate;
				Validate(session, savedata, result);
				Stage = SaveStage.Test;
				Test(session, savedata, result);
				Stage = SaveStage.Save;
				Save(session, savedata, result);
				Stage = SaveStage.AfterSave;
				AfterSave(session, savedata, result);
				Stage = SaveStage.Finished;
				return result;
			}
			catch (Exception ex) {
				RollBack(session, savedata, result, ex);
				Error = ex;
				throw;
			}
			finally {
				LastSaveResult = result;
			}
		}

		/// <summary>
		/// 	Выполняет операции после сохранения основных ячеек
		/// </summary>
		/// <param name="session"> </param>
		/// <param name="savedata"> </param>
		/// <param name="result"> </param>
		protected abstract void AfterSave(IFormSession session, XElement savedata, SaveResult result);

		/// <summary>
		/// 	Выполняет основное сохранение в хранилище
		/// </summary>
		/// <param name="session"> </param>
		/// <param name="savedata"> </param>
		/// <param name="result"> </param>
		protected abstract void Save(IFormSession session, XElement savedata, SaveResult result);

		/// <summary>
		/// 	Выполняет проверку верности вызова
		/// </summary>
		/// <param name="session"> </param>
		/// <param name="savedata"> </param>
		/// <param name="result"> </param>
		protected virtual void Validate(IFormSession session, XElement savedata, SaveResult result) {
			foreach (var sc in result.SaveCells) {
				if (null == sc.linkedcell) {
					throw new Exception("invalid id for cell " + sc.i + " not found in session");
				}
				if (!sc.linkedcell.canbefilled) {
					throw new Exception("invalid cell " + sc.i + " try to save not fillable cell");
				}
				if (null == sc.linkedcell.query) {
					throw new Exception("something wrong with session not corresponding query def on " + sc.i);
				}
				if (0 == sc.linkedcell.query.Row.Id) {
					throw new Exception("cannot save to 0 ID row");
				}
				if (0 == sc.linkedcell.query.Col.Id) {
					throw new Exception("cannot save to 0 ID col");
				}
				if (0 == sc.linkedcell.query.Obj.Id) {
					throw new Exception("cannot save to 0 Obj col");
				}
				if (20 > sc.linkedcell.query.Time.Year) {
					throw new Exception("cannot save to undefined or delta Year");
				}
				if (0 >= sc.linkedcell.query.Time.Period) {
					throw new Exception("cannot save to undefined or delta Period");
				}
			}
		}

		/// <summary>
		/// 	Выполняет подготовку внутренних объектов перед сохранением
		/// </summary>
		/// <param name="session"> </param>
		/// <param name="savedata"> </param>
		/// <param name="result"> </param>
		protected virtual void Prepare(IFormSession session, XElement savedata, SaveResult result) {
			result.SaveCells =
				savedata.Elements().Select(_ => new OutCell {v = _.Attribute("value").Value, i = _.Attribute("id").Value}).ToArray();
			foreach (var  sc in result.SaveCells) {
				sc.linkedcell = session.Data.FirstOrDefault(_ => _.i == sc.i);
			}
		}

		/// <summary>
		/// 	Проверяет техническую возможность выполнения сохранения
		/// </summary>
		/// <param name="session"> </param>
		/// <param name="savedata"> </param>
		/// <param name="result"> </param>
		protected virtual void Test(IFormSession session, XElement savedata, SaveResult result) {
			//STUB IS NOT PROBLEM
		}

		/// <summary>
		/// 	Выполняет авторизацию сохранения
		/// </summary>
		/// <param name="session"> </param>
		/// <param name="savedata"> </param>
		/// <param name="result"> </param>
		protected virtual void Authorize(IFormSession session, XElement savedata, SaveResult result) {
			var cansave = session.GetCurrentLockInfo().cansave;
			if (!cansave) {
				throw new SecurityException("try to save into closed form");
			}
			if (!Application.Roles.IsInRole(Application.Principal.CurrentUser, "OPERATOR")) {
				throw new SecurityException("try to save without OPERATOR role");
			}
			var templaterole = session.Template.Role;
			if (!string.IsNullOrWhiteSpace(templaterole)) {
				if (!Application.Roles.IsInRole(Application.Principal.CurrentUser, session.Template.Role)) {
					throw new SecurityException("try to save without " + session.Template.Role + " role");
				}
			}
		}

		/// <summary>
		/// 	Должен выполнить откат операции сохранения в случае ошибки
		/// </summary>
		/// <param name="session"> </param>
		/// <param name="savedata"> </param>
		/// <param name="result"> </param>
		/// <param name="exception"> </param>
		protected virtual void RollBack(IFormSession session, XElement savedata, SaveResult result, Exception exception) {
			//STUB IS NOT PROBLEM
		}
	}
}