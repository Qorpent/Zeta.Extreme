using System;
using System.Collections.Generic;
using System.Linq;
using Comdiv.Extensions;
using Comdiv.Zeta.Model;

using Comdiv.Zeta.Model;namespace Zeta.Extreme.Poco {
    public class form : IForm{
        #region IForm Members

        public virtual int Year { get; set; }

        public virtual int Period { get; set; }

        public virtual string Template { get; set; }
        public virtual string TemplateCode { get; set; }

        public virtual IList<IFormState> States { get; set; }

        public virtual string CurrentState { get; set; }

        public virtual int Id { get; set; }


        public virtual string Code { get; set; }


        public virtual DateTime Version { get; set; }


        public virtual IZetaMainObject Object { get; set; }


        public virtual FormStates State{
            get{
                if (CurrentState.noContent()){
                    return FormStates.None;
                }
                if (CurrentState == "0ISOPEN"){
                    return FormStates.Open;
                }
                if (CurrentState == "0ISBLOCK"){
                    return FormStates.Closed;
                }
                if (CurrentState == "0ISCHECKED"){
                    return FormStates.Accepted;
                }
                if (CurrentState == "0ISREJECTED"){
                    return FormStates.Rejected;
                }
                return FormStates.None;
            }
        }

        public virtual IFormState GetLastState() {
            var result = this.States!=null? this.States.OrderBy(x => x.Version).LastOrDefault() :null;
            if(null==result) {
                result = new formstate();
                result.Form = this;
                result.State = "0ISOPEN";
                result.Usr = "система";
            }
            return result;
        }
        public virtual IFormState GetLastBlock()
        {
            var result = this.States!=null? this.States.Where(x=>x.State=="0ISBLOCK").OrderBy(x => x.Version).LastOrDefault():null;
            if (null == result)
            {
                result = new formstate();
                result.Form = this;
                result.State = "0ISOPEN";
                result.Usr = "система";
            }
            return result;
        }
         

        #endregion
    }
}