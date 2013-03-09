#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : EcoThema.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Comdiv.Application;
using Comdiv.Extensions;
using Comdiv.Inversion;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.BizProcess.Themas;
using Zeta.Extreme.Meta;
using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme.Form.Themas {
	/// <summary>
	/// 	����������� ��� -����
	/// </summary>
	public class EcoThema : Thema {
		/// <summary>
		/// 	����������
		/// </summary>
		public IZetaUnderwriter[] Underwriters { get; set; }

		/// <summary>
		/// 	���������
		/// </summary>
		public IZetaUnderwriter[] Operators { get; set; }

		/// <summary>
		/// 	�������� ����
		/// </summary>
		public IZetaUnderwriter[] Readers { get; set; }

		/// <summary>
		/// 	������� ����
		/// </summary>
		public string Roleprefix {
			get { return Parameters.get("roleprefix", ""); }
			set { Parameters["roleprefix"] = value; }
		}

		/// <summary>
		/// 	��������������� ��������
		/// </summary>
		public IUsrThemaMap Responsibility {
			get { return Parameters.get<IUsrThemaMap>("responsibility", null); }
			set { Parameters["responsibility"] = value; }
		}

		/// <summary>
		/// 	��������������� ��������������
		/// </summary>
		public IUsrThemaMap Responsibility2 {
			get { return Parameters.get<IUsrThemaMap>("responsibility2", null); }
			set { Parameters["responsibility2"] = value; }
		}

		/// <summary>
		/// 	�������������� ��������
		/// </summary>
		public IUsrThemaMap HoldResponsibility {
			get { return Parameters.get<IUsrThemaMap>("holdresponsibility", null); }
			set { Parameters["holdresponsibility"] = value; }
		}

		/// <summary>
		/// 	������� ������������ �������� ������� ���
		/// </summary>
		public bool InvalidTargetObject {
			get { return Parameters.get("invalidtargetobject", false); }
			set { Parameters["invalidtargetobject"] = value; }
		}

		/// <summary>
		/// 	������� ���� ��� ������
		/// </summary>
		public bool IsDetail {
			get { return Parameters.get("isdetail", false); }
			set { Parameters["isdetail"] = value; }
		}

		/// <summary>
		/// 	����� ��� �������
		/// </summary>
		public string DetailClasses {
			get { return Parameters.get("detailclasses", ""); }
			set { Parameters["detailclasses"] = value; }
		}

		/// <summary>
		/// 	�������� ������
		/// </summary>
		public string RootRow {
			get { return Parameters.get("rootrow", ""); }
			set { Parameters["rootrow"] = value; }
		}

		/// <summary>
		/// 	������� �������������� � ������
		/// </summary>
		public string ForGroup {
			get { return Parameters.get("forgroup", ""); }
			set { Parameters["forgroup"] = value; }
		}

		/// <summary>
		/// 	���������� ������� ���������������
		/// </summary>
		public bool NeedResponsibility {
			get { return Parameters.get("needresponsibility", false); }
			set { Parameters["needresponsibility"] = value; }
		}

		/// <summary>
		/// 	��������� ����
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
			//���� ��� ����������� �� ������ - ������ ��� � �������, �����...

			if (obj.GroupCache.IsEmpty()) {
				return false;
			}
			//���� ������ �� � ������ - ��� �����, �����...

			var groups = ForGroup.SmartSplit();
			foreach (var s in groups) {
				foreach (var link in obj.GroupCache.SmartSplit(false, true, '/')) {
					if (link == s) {
						return true;
					}
				}
			}
			//���� ������  ������ ���� � ���� ������ - ������...


			return false;
			//�� � ����� ������ ������� - ������ �� ��������
		}

		/// <summary>
		/// 	�������� ����������
		/// </summary>
		/// <param name="principal"> </param>
		/// <returns> </returns>
		protected override bool internalIsActive(IPrincipal principal) {
			return new EcoThemaHelper(this).isvisible;
		}

		/// <summary>
		/// 	������������ ���� ��� ���������� ������
		/// </summary>
		/// <param name="obj"> </param>
		/// <param name="year"> </param>
		/// <param name="period"> </param>
		/// <returns> </returns>
		public override IThema Accomodate(IZetaMainObject obj, int year, int period) {
			var result = (EcoThema) base.Accomodate(obj, year, period);
			if (obj != null) {
				if (result.NeedResponsibility) {
					result.Responsibility = myapp.Container.get<IUsrThemaMapRepository>().GetResponsibility(Code, "Default", obj);
					result.Responsibility2 = myapp.Container.get<IUsrThemaMapRepository>().GetResponsibility2(Code, "Default", obj);
				}
				var h = MetaCache.Default.Get<IZetaMainObject>("0CH");
				if (null != h) {
					result.HoldResponsibility =
						myapp.Container.get<IUsrThemaMapRepository>().GetResponsibility(Code, "Default", h);
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
		/// 	���������� ������� (�����??)
		/// </summary>
		public void SetupDetails() {
			IList<IZetaUnderwriter> unds = new List<IZetaUnderwriter>();
			IList<IZetaUnderwriter> ops = new List<IZetaUnderwriter>();
			IList<IZetaUnderwriter> reads = new List<IZetaUnderwriter>();
			var ur = Roleprefix + "_UNDERWRITER";
			var wr = Roleprefix + "_OPERATOR";
			var rr = Roleprefix + "_ANALYTIC";
			Func<IZetaUnderwriter, string, bool> inrole = (x, s) =>
				{
					if (x.Roles.IsEmpty()) {
						return false;
					}
					return x.Roles.Contains(s);
				};
			foreach (var o in Object.Underwriters) {
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
			Underwriters = unds.OrderBy(x => x.Name).ToArray();
			Operators = ops.OrderBy(x => x.Name).ToArray();
			Readers = reads.OrderBy(x => x.Name).ToArray();
		}
	}
}