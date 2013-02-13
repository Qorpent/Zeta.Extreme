using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comdiv.Olap.Model;
using Comdiv.Zeta.Data;

namespace Comdiv.Zeta.Web.Themas {
    public interface IResponsibilityProvider {
        string Get(RowDescriptor row, IZoneElement obj, ColumnDesc col);
    }
}
