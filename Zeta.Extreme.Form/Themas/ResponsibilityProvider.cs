using Comdiv.Application;
using Comdiv.Model;
using Comdiv.Olap.Model;
using Comdiv.Zeta.Data;
using Comdiv.Zeta.Model;

namespace Comdiv.Zeta.Web.Themas {
    public  class ResponsibilityProvider : IResponsibilityProvider {
        public string Get(RowDescriptor row, IZoneElement obj, ColumnDesc col) {
          //  return row.ToString() + obj + col;


            var pf = col.Code;
            var suffix = pf == "F" ? "" : "_2";
            var themacode = row.Code + suffix;
            var obj_ = obj as IZetaMainObject;
            if(obj_==null) {
                return "";
            }
            var usrmap = myapp.storage.Get<IUsrThemaMap>().First("from ENTITY x where x.Object=? and x.Thema = ?",obj_.Id,themacode);
            if(usrmap==null) {
                return string.Format("{0}/{1}",themacode,obj_.Id);
            }
            return usrmap.Usr.Code;
        }
    }
}