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
using Comdiv.Application;
using Comdiv.Model;

using Comdiv.Zeta.Model;namespace Zeta.Extreme.Poco{
    public class formstate : IFormState{
        #region IFormState Members

        public virtual IForm Form { get; set; }

        public virtual string State { get; set; }

        public virtual string Usr { get; set; }

        public virtual IFormState Parent { get; set; }
        public virtual string GetReadableState() {
            switch (State) {
                case "0ISOPEN":
                    return "�������";
                case "0ISBLOCK":
                    return "�������";
                case "0ISCHECKED":
                    return "���������";
            }
            return State;
        }

        public virtual int Id { get; set; }

        public virtual DateTime Version { get; set; }

        public virtual string Comment { get; set; }
		/// <summary>
		/// ������������� �����
		/// </summary>
	    public int FormId { get; set; }
		/// <summary>
		/// ������������� ������������� �������
		/// </summary>
	    public int ParentId { get; set; }

	    #endregion
    }

    public class Pkg : IPkg{
        public virtual Guid Uid { get; set; }

        #region IPkg Members

        public virtual string Tag { get; set; }
        public virtual IList<IZetaCell> Cells { get; set; }

        public virtual string Code { get; set; }

        public virtual string Comment { get; set; }

        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        [Ref(Alias = "PkgType", Title = "���")]
        public virtual IPkgType Type { get; set; }

        public virtual DateTime Version { get; set; }

        [Ref(Alias = "Pkg", Title = "������������ �����", Nullable = true, IsAutoComplete = true,
            AutoCompleteType = "pkg")]
        public virtual IPkg Parent { get; set; }

        public virtual IList<IPkg> Children { get; set; }

        [Ref(Alias = "Org", Title = "������", Nullable = true, IsAutoComplete = true, AutoCompleteType = "org")]
        public virtual IZetaDetailObject DetailObject { get; set; }

        [Ref(Alias = "Subpart", Title = "������", Nullable = true, IsAutoComplete = true, AutoCompleteType = "sp")]
        public virtual IZetaMainObject Object { get; set; }

        [Map(Title = "���������", CustomType = typeof(int))]
        public virtual PkgState State { get; set; }

        [Map(Title = "����")]
        public virtual DateTime Date { get; set; }

        [Map(Title = "�����")]
        public virtual string Number { get; set; }


        [Map(Title = "���� ��������")]
        public virtual DateTime CreateTime { get; set; }

        [Map(Title = "������������")]
        public virtual string Usr { get; set; }

        public virtual bool IsClosed(){
            if (State == PkgState.Closed){
                return true;
            }
            if (Parent != null){
                return Parent.IsClosed();
            }
            return false;
        }

        #endregion

        public override string ToString(){
            return string.Format("pkg:{0}/{1}/{2}/{3}/{4}", Type.Code(), Object.Code(),
                                 Date.ToString("yyyy-MM-dd"),
                                 Number, Code);
        }
    }
}