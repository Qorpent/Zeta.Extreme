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
using System.Xml.Serialization;
using Qorpent.Model;
using Qorpent.Serialization;
using Zeta.Extreme.Model.Extensions;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Model.Inerfaces.Bases;


#if NEWMODEL

#endif

namespace Zeta.Extreme.BizProcess.Themas{
	/// <summary>
	/// ��������� ��������� ��������� �������
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="R"></typeparam>
	[Serialize]
	public abstract class DimensionDescriptor<T, R> :  ICloneable where T : IWithFormula{
		/// <summary>
		/// ������ �����
		/// </summary>
		[SerializeNotNullOnly]
        [XmlAttribute] public string NumberFormat;
        private T _target;
        private string code;
        private string formula;
        
        private bool? isFormula;
        private string name;
        
		/// <summary>
		/// ����
		/// </summary>
		[SerializeNotNullOnly]
        public string Codes { get; set; }
		

		/// <summary>
		/// ������� �������������� ����
		/// </summary>
		[IgnoreSerialize]
        public bool HasTarget{
            get { return Target.ToBool(); }
        }
		/// <summary>
		/// ������� ������ ���������
		/// </summary>
		[IgnoreSerialize]
        [XmlIgnore]
        public T Target{
            get { return _target; }
            set{
                _target = value;
                if (null != value){
                    if (value.FormulaType.IsNotEmpty()){
                        Formula = value.Formula;
                        FormulaType = value.FormulaType;
                    }
                }
            }
        }
		/// <summary>
		/// ��������
		/// </summary>
		[SerializeNotNullOnly]
        public string Name{
            get { return name ?? (null==Target?"":((IWithName)Target).Name); }
            set { name = value; }
        }
		/// <summary>
		/// ������� �����������
		/// </summary>
		[SerializeNotNullOnly]
        public string Condition { get; set; }
		/// <summary>
		/// ����� CSS
		/// </summary>
		[SerializeNotNullOnly]
        public string CssClass { get; set; }
		/// <summary>
		/// ����� CSSS
		/// </summary>
		[SerializeNotNullOnly]
        public string CssStyle { get; set; }

        #region IDimension Members
		/// <summary>
		/// ���
		/// </summary>
		[SerializeNotNullOnly]
        public string Code{
            get { return code ?? (null==Target?"":((IWithCode)Target).Code); }
            set { code = value; }
        }
		/// <summary>
		/// �������
		/// </summary>
		[SerializeNotNullOnly]
        public string Formula{
            get { return formula ?? Target.Formula(); }
            set { formula = value; }
        }

		/// <summary>
		/// ��� �������
		/// </summary>
		[SerializeNotNullOnly]
        public string FormulaType{
            get { return _formulaType ?? Target.FormulaType(); }
            set { _formulaType = value; }
        }
		/// <summary>
		/// ������� �������
		/// </summary>
		[SerializeNotNullOnly]
        public bool IsFormula{
            get { return isFormula ?? Target.IsFormula(); }
            set { isFormula = value; }
        }

   

    

		private string _tag;
		/// <summary>
		/// ���
		/// </summary>
		[SerializeNotNullOnly]
		public string Tag {
			//#ZC-7
			get {
				if(_tag.IsNotEmpty()) return _tag;
				if (null != this.Target) return ((IEntity)this.Target).Tag ?? "";
				return _tag;
			}
			set { _tag = value; }
		}

		object ICloneable.Clone(){
            return Clone();
        }


        #endregion

        /// <summary>
        /// ������������
        /// </summary>
        /// <returns></returns>
        public virtual R Clone(){
            return (R) MemberwiseClone();
        }

        /// <summary>
        /// ����������� ��������� ��������
        /// </summary>
        /// <returns></returns>
        public virtual bool GetIsVisible(){
            return true;
        }

		private int _id;
		/// <summary>
		/// ��
		/// </summary>
		public int Id {
			get {
				if(0==_id) {
					if(null!=Target) {
						return ((IWithId)Target).Id;
					}
				}
				return _id;
			}
			set {
				_id = value;
			}
		}

		private string _comment;
		/// <summary>
		/// �����������
		/// </summary>
		[SerializeNotNullOnly]
        
		public string Comment {
			get { return _comment; }
			set { _comment = value; }
		}

		private DateTime _version;
		private string _formulaType;

		/// <summary>
		/// ������
		/// </summary>
		[SerializeNotNullOnly]
		public DateTime Version {
			get { return _version; }
			private set { _version = value; }
		}
		/// <summary>
		/// �������� ����
		/// </summary>
		[SerializeNotNullOnly]
		public string File { get; set; }
		/// <summary>
		/// �������� ������
		/// </summary>
		[SerializeNotNullOnly]
		public string Line { get; set; }
    }
}