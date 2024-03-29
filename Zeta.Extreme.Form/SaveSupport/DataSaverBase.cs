#region LICENSE
// Copyright 2007-2013 Qorpent Team - http://github.com/Qorpent
// Supported by Media Technology LTD 
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//  
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// PROJECT ORIGIN: Zeta.Extreme.Form/DataSaverBase.cs
#endregion
using System;
using System.Linq;
using System.Security;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Xml.Linq;
using Qorpent;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.Form.SaveSupport {
	/// <summary>
	/// 	������� ����������� ����� ��� ���������� ������, ������� - ��������� �����
	/// </summary>
	public abstract class DataSaverBase : ServiceBase, IFormSessionDataSaver {
		/// <summary>
		/// 	������� ������
		/// </summary>
		public Task<SaveResult> Current { get; set; }

		/// <summary>
		/// 	��������� ��������� ����������
		/// </summary>
		public SaveResult LastSaveResult { get; set; }

		/// <summary>
		/// 	�����
		/// </summary>
		/// <param name="session"> </param>
		/// <param name="savedata"> </param>
		/// <param name="currentUser"> </param>
		/// <returns> </returns>
		/// <exception cref="NotImplementedException"></exception>
		public Task<SaveResult> BeginSave(IFormSession session, XElement savedata, IPrincipal currentUser) {
			if (null != Current && !Current.IsCompleted) {
				Current.Wait();
			}
			Current = null;
			Stage = SaveStage.Load;
			Error = null;
			var user = currentUser;
			return (Current = Task.Run(() => InternalSave(session, savedata,user)));
		}

		/// <summary>
		/// 	������� ������ �������� ����������
		/// </summary>
		public SaveStage Stage { get; set; }

		/// <summary>
		/// 	��������� ��������� ������
		/// </summary>
		public Exception Error { get; set; }

		/// <summary>
		/// 	���������� ��������� ����������, ���������� ������ ����������
		/// </summary>
		/// <param name="session"> </param>
		/// <param name="savedata"> </param>
		/// <param name="user"> </param>
		/// <returns> </returns>
		protected SaveResult InternalSave(IFormSession session, XElement savedata, IPrincipal user) {
			Application = Application ?? Qorpent.Applications.Application.Current;
			var result = new SaveResult();
			try {
				Stage = SaveStage.Authorize;
				Authorize(session, savedata, result,user);
				Stage = SaveStage.Prepare;
				Prepare(session, savedata, result);
				Stage = SaveStage.Validate;
				Validate(session, savedata, result);
				Stage = SaveStage.Test;
				Test(session, savedata, result);
				Stage = SaveStage.Save;
				Save(session, savedata, result,user);
				Stage = SaveStage.AfterSave;
				AfterSave(session, savedata, result,user);
				Stage = SaveStage.Finished;
				return result;
			}
			catch (Exception ex) {
				RollBack(session, savedata, result, ex);
				Error = ex;
				session.Logger.Error("save finished",ex);
				throw;
			}
			finally {
				LastSaveResult = result;
				session.Logger.Info("save finished");
			}
		}

		/// <summary>
		/// 	��������� �������� ����� ���������� �������� �����
		/// </summary>
		/// <param name="session"> </param>
		/// <param name="savedata"> </param>
		/// <param name="result"> </param>
		/// <param name="user"> </param>
		protected abstract void AfterSave(IFormSession session, XElement savedata, SaveResult result, IPrincipal user);

		/// <summary>
		/// 	��������� �������� ���������� � ���������
		/// </summary>
		/// <param name="session"> </param>
		/// <param name="savedata"> </param>
		/// <param name="result"> </param>
		/// <param name="user"> </param>
		protected abstract void Save(IFormSession session, XElement savedata, SaveResult result, IPrincipal user);

		/// <summary>
		/// 	��������� �������� �������� ������
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
		/// 	��������� ���������� ���������� �������� ����� �����������
		/// </summary>
		/// <param name="session"> </param>
		/// <param name="savedata"> </param>
		/// <param name="result"> </param>
		protected virtual void Prepare(IFormSession session, XElement savedata, SaveResult result) {
			result.SaveCells =
				savedata.Elements().Select(_ => new OutCell { v = _.Attribute("value").Value, i = _.Attribute("id").Value, ri =_.Attr("ri")}).ToArray();
			foreach (var  sc in result.SaveCells) {
				sc.linkedcell = session.Data.FirstOrDefault(_ => (string.IsNullOrWhiteSpace(sc.ri) && _.i == sc.i) || (!string.IsNullOrWhiteSpace(sc.ri) && sc.i == _.i));
			}
		}

		/// <summary>
		/// 	��������� ����������� ����������� ���������� ����������
		/// </summary>
		/// <param name="session"> </param>
		/// <param name="savedata"> </param>
		/// <param name="result"> </param>
		protected virtual void Test(IFormSession session, XElement savedata, SaveResult result) {
			//STUB IS NOT PROBLEM
		}

		/// <summary>
		/// 	��������� ����������� ����������
		/// </summary>
		/// <param name="session"> </param>
		/// <param name="savedata"> </param>
		/// <param name="result"> </param>
		/// <param name="user"> </param>
		protected virtual void Authorize(IFormSession session, XElement savedata, SaveResult result, IPrincipal user) {
			var cansave = session.GetStateInfo().cansave;
			if (!cansave) {
				throw new SecurityException("try to save into closed form");
			}
			//if (!Application.Roles.IsInRole(user,session.Template.Role)) {
			//	throw new SecurityException("try to save without OPERATOR role");
			//}
			var templaterole = session.Template.Role;
			if (!string.IsNullOrWhiteSpace(templaterole)) {
				if (!Application.Roles.IsInRole(user, session.Template.Role)) {
					throw new SecurityException("try to save without " + session.Template.Role + " role");
				}
			}
		}

		/// <summary>
		/// 	������ ��������� ����� �������� ���������� � ������ ������
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