// Copyright 2007-2010 Comdiv (F. Sadykov) - http://code.google.com/u/fagim.sadykov/
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
// MODIFICATIONS HAVE BEEN MADE TO THIS FILE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Comdiv.Application;
using Comdiv.Extensions;
using Qorpent.Applications;
using Qorpent.IoC;
using Qorpent.Model;
using Qorpent.Utils.Extensions;

namespace Zeta.Extreme.BizProcess.Reports
{
	/// <summary>
	/// �������� ������
	/// </summary>
	public class Parameter
    {
		/// <summary>
		/// ���������� ������, ������� ������������ ������� ������.
		/// </summary>
		/// <returns>
		/// ������, �������������� ������� ������.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override string ToString()
        {
            return string.Format(
                "{0}->{4}, temp:{1}, type:{2}, def:{3}",
                Code, !Static, Type, DefaultValue,RealTarget
                );
        }
		/// <summary>
		/// �������������� �������� ��� bool
		/// </summary>
        public string AltValue { get; set; }
		/// <summary>
		/// ��� ���������
		/// </summary>
        public string Name { get; set; }
		/// <summary>
		/// ��� ������������� ���������
		/// </summary>
        public string CustomView { get; set; }
		/// <summary>
		/// ������� �����������
		/// </summary>
        public bool Radio { get; set; }
		/// <summary>
		/// ������ ����������
		/// </summary>
        public string Group { get; set; }
		/// <summary>
		/// ������� CSS-���������
		/// </summary>
        public bool IsCss { get; set; }
		/// <summary>
		/// ������� �������� ������
		/// </summary>
        public bool IsArea { get; set; }
		/// <summary>
		/// ������� �������� ���������
		/// </summary>
        public bool IsHidden { get; set; }
		/// <summary>
		/// �������� ���������
		/// </summary>
        public string Tab { get; set; }
		

		/// <summary>
		/// 
		/// </summary>
        public Parameter()
        {
            Static = true;
            this.RealType = typeof (string);
            ValueList = new List<Entity>();
        }
		/// <summary>
		/// ������� ������������ ���������
		/// </summary>
        public bool Static { get; set; }
        
		/// <summary>
		/// �������� ��������� �������� �� ���������
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
        public Parameter SetRawValue(object value)
        {
            this.RawValue = value;
            return this;
        }

    	private string _type;
        /// <summary>
        /// ��� ���� ���������
        /// </summary>
        public string Type
        {
            get { return _type; }
            set {
            	_type = value;
                RealType = value.ToTypeDefinition();
            }
        }

		/// <summary>
		/// ��� �������� ���������
		/// </summary>
        public string Target { get; set; }
		/// <summary>
		/// �������� ������� �������� ������
		/// </summary>
        public string RealTarget
        {
            get
            {
                return Target.IsNotEmpty() ? Target : (Code.IsNotEmpty() ? Code : "NONE");
            }
        }
		/// <summary>
		/// �������� ��������
		/// </summary>
        public object RawValue { get; set; }
		/// <summary>
		/// �������� ��������
		/// </summary>
        public object Value {
            get
            {
               var result = internalGetValue();
                return result ?? "";
            }
        }

        private object internalGetValue()
        {
            
            var val = RawValue ?? DefaultValue;

            if(this.Type=="bool" && this.AltValue.IsNotEmpty()){
                var yes = val.ToBool();
                if(yes){
                    return AltValue;
                }else{
                    return "";
                }
            }

            if(null==val  && null!=DefaultValue)
            {
                val = DefaultValue;
            }
            if(typeof(string)==RealType)
            {
                return val.ToStr();
            }
            if(val==null||!(val is string) || !((string)val).Contains(","))
            {
                return val.ToType(RealType);
            }
            return ((string)val).SmartSplit(false, true, ',').Select(x => x.to(RealType)).ToArray();
        }

		
		/// <summary>
		/// �������� �� ���������
		/// </summary>
        public object DefaultValue { get; set; }

        private Type _realType;

		/// <summary>
		/// �������� ��� ���������
		/// </summary>
        public Type RealType
        {
            get { return _realType ?? (_realType = typeof(string)); }
            set { _realType = value; }
        }
		/// <summary>
		/// ��� ���������
		/// </summary>
        public string Code
        {
            get; set;
        }

        private IContainer _container;
		/// <summary>
		/// ��������� ����������
		/// </summary>
        public IContainer Container{
            get{
                
                return _container ?? (_container = Application.Current.Container);
            }
            set { _container = value; }
        }

        private string __listDefinition;
        /// <summary>
        /// ������������ ������
        /// </summary>
        /// <exception cref="NotSupportedException"></exception>
        public string ListDefinition
        {
            get { return __listDefinition; }
            set
            {
                __listDefinition = value;
                if (__listDefinition.StartsWith("$hql:"))
                {
	                throw new NotSupportedException("hql parameters not supported by Zeta.Extreme for now");
                    
                }
                else
                {
                    ValueList = (from s in __listDefinition.Split('|')
                                 let def = s.Trim()
                                 let rec = def.Split(':')
                                 let uni = rec.Length == 1
                                 let code = uni ? def : rec[0]
                                 let name = uni ? def : rec[1]
                                 select new Entity { Code = code.Trim(), Name = name.Trim() }).ToList();
                }
            }
        }
		/// <summary>
		/// ������ ��������
		/// </summary>
        public IList<Entity> ValueList { get; set; }
		/// <summary>
		/// ������ ���������
		/// </summary>
        public int Idx { get; set; }
		/// <summary>
		/// ���� �������
		/// </summary>
        public string Role { get; set; }
		/// <summary>
		/// ������� ???
		/// </summary>
        public string Level { get; set; }
		/// <summary>
		/// ������ ����� ���������
		/// </summary>
		/// <returns></returns>
        public Parameter Clone(){
            var result =(Parameter) this.MemberwiseClone();
            return result;
        }

		/// <summary>
		/// ������� ������� �������
		/// </summary>
		/// <returns></returns>
        public bool HasHelp() {
            throw new NotSupportedException("wiki help not now supported");
        }
		/// <summary>
		/// ����������� ���������
		/// </summary>
		/// <param name="usr"></param>
		/// <returns></returns>
        public bool Authorize(IPrincipal usr) {
            var param = this;
            if (param.Role.hasContent())
            {
                var roles = param.Role.split().ToList();
                var strict = false;
                var remove = true;
                if (roles.Contains("STRICT"))
                {
                    strict = true;
                    roles.Remove("STRICT");
                }
                if (roles.Contains("REMOVE"))
                {
                    remove = true;
                    roles.Remove("REMOVE");
                }

                bool isadmin = Application.Current.Roles.IsInRole(usr, "ADMIN", true);
                bool isinrole = isadmin;
                if (!isinrole)
                {
                    if (roles.Any(role => Application.Current.Roles.IsInRole(usr, role, true))) {
	                    isinrole = true;
                    }
                }
                if ((strict && isinrole) || (!strict && !isinrole))
                {
                    if (remove) {
                        return false;
                    }
                    
                }
            }
            return true;
        }
		/// <summary>
		/// ������� ������� ��������� �� ����������
		/// </summary>
		/// <param name="libcode"></param>
		/// <returns></returns>
    	public Parameter MarkAsFromLib(string libcode) {
    		this.FromLibrary = true;
    		this.Library = libcode;
    		return this;
    	}
		/// <summary>
		/// ����������
		/// </summary>
    	public string Library { get; set; }
		/// <summary>
		/// ������� ������� ��������� �� ����������
		/// </summary>
    	public bool FromLibrary { get; set; }
    }
}