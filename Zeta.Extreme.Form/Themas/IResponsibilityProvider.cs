using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comdiv.Olap.Model;
using Comdiv.Zeta.Data;
using Comdiv.Zeta.Data.Minimal;

namespace Comdiv.Zeta.Web.Themas {
	/// <summary>
	/// Интерфейс провайдера строк напоминания о заполнении
	/// </summary>
    public interface IResponsibilityProvider {
		/// <summary>
		/// Получить строку напоминания
		/// </summary>
		/// <param name="row"></param>
		/// <param name="obj"></param>
		/// <param name="col"></param>
		/// <returns></returns>
        string Get(RowDescriptor row, IZoneElement obj, ColumnDesc col);
    }
	//NOTE: что это вообще за интерфейс такой - со строками, колонками, объектами
	// откровенный бред
}
