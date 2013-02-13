// // Copyright 2007-2010 Comdiv (F. Sadykov) - http://code.google.com/u/fagim.sadykov/
// // Supported by Media Technology LTD 
// //  
// // Licensed under the Apache License, Version 2.0 (the "License");
// // you may not use this file except in compliance with the License.
// // You may obtain a copy of the License at
// //  
// //      http://www.apache.org/licenses/LICENSE-2.0
// //  
// // Unless required by applicable law or agreed to in writing, software
// // distributed under the License is distributed on an "AS IS" BASIS,
// // WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// // See the License for the specific language governing permissions and
// // limitations under the License.
// // 
// // MODIFICATIONS HAVE BEEN MADE TO THIS FILE
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Comdiv.Application;
using Comdiv.Extensions;
using Comdiv.Inversion;
using Comdiv.Zeta.Data;
using Comdiv.Zeta.Data.Minimal;
using Comdiv.Zeta.Model;

namespace Comdiv.Zeta.Web.Themas{
	/// <summary>
	/// ����������� ��� -����
	/// </summary>
    public class EcoThema : Thema{
		/// <summary>
		/// ����������
		/// </summary>
        public IZetaUnderwriter[] Underwriters { get; set; }
		/// <summary>
		/// ���������
		/// </summary>
        public IZetaUnderwriter[] Operators { get; set; }
		/// <summary>
		/// �������� ����
		/// </summary>
        public IZetaUnderwriter[] Readers { get; set; }
		/// <summary>
		/// ������� ����
		/// </summary>
        public string Roleprefix{
            get { return Parameters.get("roleprefix", ""); }
            set { Parameters["roleprefix"] = value; }
        }

        /// <summary>
        ///   Determines whether the specified obj is match.
        /// </summary>
        /// <param name = "obj">The obj.</param>
        /// <returns>
        ///   <c>true</c> if the specified obj is match; otherwise, <c>false</c>.
        /// </returns>
        public bool IsMatch(IZetaMainObject obj)
        {
            if (ForGroup.noContent())
            {
                return true;
            }
            //���� ��� ����������� �� ������ - ������ ��� � �������, �����...

            if (obj.GroupCache.noContent())
            {
                return false;
            }
            //���� ������ �� � ������ - ��� �����, �����...

            IList<string> groups = ForGroup.split();
            foreach (string s in groups)
            {
                foreach (var link in obj.GroupCache.split(false, true, '/'))
                {
                    if (link == s)
                    {
                        return true;
                    }
                }
            }
            //���� ������  ������ ���� � ���� ������ - ������...


            return false;
            //�� � ����� ������ ������� - ������ �� ��������
        }
		/// <summary>
		/// ��������������� ��������
		/// </summary>
        public IUsrThemaMap Responsibility{
            get { return Parameters.get<IUsrThemaMap>("responsibility", null); }
            set { Parameters["responsibility"] = value; }
        }
		/// <summary>
		/// ��������������� ��������������
		/// </summary>
        public IUsrThemaMap Responsibility2{
            get { return Parameters.get<IUsrThemaMap>("responsibility2", null); }
            set { Parameters["responsibility2"] = value; }
        }

		/// <summary>
		/// �������������� ��������
		/// </summary>
        public IUsrThemaMap HoldResponsibility{
            get { return Parameters.get<IUsrThemaMap>("holdresponsibility", null); }
            set { Parameters["holdresponsibility"] = value; }
        }
		/// <summary>
		/// ������� ������������ �������� ������� ��� 
		/// </summary>
        public bool InvalidTargetObject{
            get { return Parameters.get("invalidtargetobject", false); }
            set { Parameters["invalidtargetobject"] = value; }
        }
		/// <summary>
		/// ������� ���� ��� ������
		/// </summary>
        public bool IsDetail{
            get { return Parameters.get("isdetail", false); }
            set { Parameters["isdetail"] = value; }
        }
		/// <summary>
		/// ����� ��� �������
		/// </summary>
        public string DetailClasses{
            get { return Parameters.get("detailclasses", ""); }
            set { Parameters["detailclasses"] = value; }
        }
		/// <summary>
		/// �������� ������
		/// </summary>
        public string RootRow{
            get { return Parameters.get("rootrow", ""); }
            set { Parameters["rootrow"] = value; }
        }
		/// <summary>
		/// ������� �������������� � ������
		/// </summary>
        public string ForGroup{
            get { return Parameters.get("forgroup", ""); }
            set { Parameters["forgroup"] = value; }
        }

		/// <summary>
		/// ���������� ������� ���������������
		/// </summary>
        public bool NeedResponsibility{
            get { return Parameters.get("needresponsibility", false); }
            set { Parameters["needresponsibility"] = value; }
        }

    	/// <summary>
    	/// ��������� ����
    	/// </summary>
    	public IThema GroupThema { get; set; }
		/// <summary>
		/// �������� ����������
		/// </summary>
		/// <param name="principal"></param>
		/// <returns></returns>
    	protected override bool internalIsActive(IPrincipal principal){
            return new EcoThemaHelper(this).isvisible;
        }

		/// <summary>
		/// ������������ ���� ��� ���������� ������
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="year"></param>
		/// <param name="period"></param>
		/// <returns></returns>
		public override IThema Accomodate(IZetaMainObject obj, int year, int period){
            var result = (EcoThema) base.Accomodate(obj, year, period);
            if (obj != null){
                if (result.NeedResponsibility){
                    result.Responsibility = myapp.Container.get<IUsrThemaMapRepository>().GetResponsibility(Code, "Default", obj);
					result.Responsibility2 = myapp.Container.get<IUsrThemaMapRepository>().GetResponsibility2(Code, "Default", obj);
                }
                var h = "0CH".asObject();
                if (null != h){
                    result.HoldResponsibility =
						myapp.Container.get<IUsrThemaMapRepository>().GetResponsibility(Code, "Default", h);
                }
                if (obj.Id() == h.Id()){
                    result.Responsibility = result.HoldResponsibility;
                    result.Responsibility2 = result.HoldResponsibility;
                }
            }

            if (result.ForGroup.hasContent()) {
            	result.InvalidTargetObject = !GroupFilterHelper.IsMatch(obj,ForGroup);
            }
            return result;
        }

        /// <summary>
        /// ���������� ������� (�����??)
        /// </summary>
        public void SetupDetails(){
            IList<IZetaUnderwriter> unds = new List<IZetaUnderwriter>();
            IList<IZetaUnderwriter> ops = new List<IZetaUnderwriter>();
            IList<IZetaUnderwriter> reads = new List<IZetaUnderwriter>();
            var ur = Roleprefix + "_UNDERWRITER";
            var wr = Roleprefix + "_OPERATOR";
            var rr = Roleprefix + "_ANALYTIC";
            Func<IZetaUnderwriter, string, bool> inrole = (x, s) =>{
                                                              if (x.Roles.noContent()){
                                                                  return false;
                                                              }
                                                              return x.Roles.Contains(s);
                                                          };
            foreach (var o in Object.Underwriters){
                if (inrole(o, ur)){
                    unds.Add(o);
                    continue;
                }
                if (inrole(o, wr)){
                    ops.Add(o);
                    continue;
                }
                if (inrole(o, rr)){
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