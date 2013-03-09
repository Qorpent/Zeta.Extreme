using System.Collections.Generic;
using Comdiv.Model.Interfaces;
using Comdiv.Olap.Model;

namespace Comdiv.Zeta.Model {
	public interface IZetaQueryDimension : IEntityDataPattern,IWithFormula {
		IDictionary<string, object> LocalProperties { get; }
	}
}