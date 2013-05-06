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
// PROJECT ORIGIN: Zeta.Extreme.Form/EcoThema.cs
#endregion
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Principal;
using Qorpent.Applications;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.BizProcess.Themas;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Deprecated;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.SqlSupport;

namespace Zeta.Extreme.Form.Themas {
	/// <summary>
	/// 	Стандартная ЭКО -тема
	/// </summary>
	[Obsolete("very bad wrapper")]
	public class EcoThema : Thema {
		/// <summary>
		/// 	Подписанты
		/// </summary>
		public IZetaUser[] Users { get; set; }

		/// <summary>
		/// 	Операторы
		/// </summary>
		public IZetaUser[] Operators { get; set; }

		/// <summary>
		/// 	Читатели темы
		/// </summary>
		public IZetaUser[] Readers { get; set; }

		/// <summary>
		/// 	Префикс роли
		/// </summary>
		public string Roleprefix {
			get { return Parameters.SafeGet("roleprefix", ""); }
			set { Parameters["roleprefix"] = value; }
		}

		/// <summary>
		/// 	Ответственность основная
		/// </summary>

		public IUserBizCaseMap Responsibility {
			get { return Parameters.SafeGet<IUserBizCaseMap>("responsibility"); }
			set { Parameters["responsibility"] = value; }
		}

		/// <summary>
		/// 	Ответственность дополнительная
		/// </summary>
		public IUserBizCaseMap Responsibility2 {
			get { return Parameters.SafeGet<IUserBizCaseMap>("responsibility2"); }
			set { Parameters["responsibility2"] = value; }
		}

		/// <summary>
		/// 	Отвественность холдинга
		/// </summary>
		public IUserBizCaseMap HoldResponsibility {
			get { return Parameters.SafeGet<IUserBizCaseMap>("holdresponsibility"); }
			set { Parameters["holdresponsibility"] = value; }
		}

		/// <summary>
		/// 	Признак невалидности целевого объекта для
		/// </summary>
		public bool InvalidTargetObject {
			get { return Parameters.SafeGet("invalidtargetobject", false); }
			set { Parameters["invalidtargetobject"] = value; }
		}

		/// <summary>
		/// 	Признак темы для детали
		/// </summary>
		public bool IsDetail {
			get { return Parameters.SafeGet("isdetail", false); }
			set { Parameters["isdetail"] = value; }
		}

		/// <summary>
		/// 	Класс для деталей
		/// </summary>
		public string DetailClasses {
			get { return Parameters.SafeGet("detailclasses", ""); }
			set { Parameters["detailclasses"] = value; }
		}
	

		/// <summary>
		/// 	Корневая строка
		/// </summary>
		public string RootRow {
			get { return Parameters.SafeGet("rootrow", ""); }
			set { Parameters["rootrow"] = value; }
		}

		/// <summary>
		/// 	Признак принадлежности к группе
		/// </summary>
		public string ForGroup {
			get { return Parameters.SafeGet("forgroup", ""); }
			set { Parameters["forgroup"] = value; }
		}

		/// <summary>
		/// 	Требование наличия ответственности
		/// </summary>
		public bool NeedResponsibility {
			get { return Parameters.SafeGet("needresponsibility", false); }
			set { Parameters["needresponsibility"] = value; }
		}

		/// <summary>
		/// 	Групповая тема
		/// </summary>
		public IThema GroupThema { get; set; }

		/// <summary>
		/// 	Determines whether the specified obj is match.
		/// </summary>
		/// <param name="obj"> The obj. </param>
		/// <returns> <c>true</c> if the specified obj is match; otherwise, <c>false</c> . </returns>
		public bool IsMatch(IZetaMainObject obj) {
			if (ForGroup.IsEmpty()) {
				return true;
			}
			//если нет ограничений на группу - значит все в порядке, иначе...

			if (obj.GroupCache.IsEmpty()) {
				return false;
			}
			//если объект не в группе - все плохо, инача...

			var groups = ForGroup.SmartSplit();
			foreach (var s in groups) {
				foreach (var link in obj.GroupCache.SmartSplit(false, true, '/')) {
					if (link == s) {
						return true;
					}
				}
			}
			//если объект  входит хоть в одну группу - хорошо...


			return false;
			//но в любых прочих случаях - шаблон не подходит
		}

		/// <summary>
		/// 	Проверка активности
		/// </summary>
		/// <param name="principal"> </param>
		/// <returns> </returns>
		protected override bool internalIsActive(IPrincipal principal) {
			return new EcoThemaHelper(this).isvisible;
		}

		/// <summary>
		/// 	Приспособить тему под конкретный период
		/// </summary>
		/// <param name="obj"> </param>
		/// <param name="year"> </param>
		/// <param name="period"> </param>
		/// <returns> </returns>
		public override IThema Accomodate(IZetaMainObject obj, int year, int period) {
			var result = (EcoThema) base.Accomodate(obj, year, period);
			if (obj != null) {
				if (result.NeedResponsibility) {
					result.Responsibility = Application.Current.Container.Get<IUsrThemaMapRepository>().GetResponsibility(Code, "Default", obj);
					result.Responsibility2 = Application.Current.Container.Get<IUsrThemaMapRepository>().GetResponsibility2(Code, "Default", obj);
				}
				var h = MetaCache.Default.Get<IZetaMainObject>("0CH");
				if (null != h) {
					result.HoldResponsibility =
						Application.Current.Container.Get<IUsrThemaMapRepository>().GetResponsibility(Code, "Default", h);
				}
				if (obj.Id == h.Id) {
					result.Responsibility = result.HoldResponsibility;
					result.Responsibility2 = result.HoldResponsibility;
				}
			}

			if (result.ForGroup.IsNotEmpty()) {
				result.InvalidTargetObject = !GroupFilterHelper.IsMatch(obj, ForGroup);
			}
			return result;
		}

		/// <summary>
		/// 	Подготовка деталей (ролей??)
		/// </summary>
		public void SetupDetails() {
			IList<IZetaUser> unds = new List<IZetaUser>();
			IList<IZetaUser> ops = new List<IZetaUser>();
			IList<IZetaUser> reads = new List<IZetaUser>();
			var ur = Roleprefix + "_UNDERWRITER";
			var wr = Roleprefix + "_OPERATOR";
			var rr = Roleprefix + "_ANALYTIC";
			Func<IZetaUser, string, bool> inrole = (x, s) =>
				{
					if (x.Roles.IsEmpty()) {
						return false;
					}
					return x.Roles.Contains(s);
				};
			foreach (var o in Object.Users) {
				if (inrole(o, ur)) {
					unds.Add(o);
					continue;
				}
				if (inrole(o, wr)) {
					ops.Add(o);
					continue;
				}
				if (inrole(o, rr)) {
					reads.Add(o);
					continue;
				}
			}
			Users = unds.OrderBy(x => x.Name).ToArray();
			Operators = ops.OrderBy(x => x.Name).ToArray();
			Readers = reads.OrderBy(x => x.Name).ToArray();
		}
	}
}