using System.Collections.Generic;

namespace Zeta.Themas.Loader.Abstracts {
	public interface IFormThemaItem : IThemaItem {
		string LockCode { get; set; }
		List<IFormLockDepend> InLockDepends { get; }
		List<IFormLockDepend> OutLockDepends { get; }
	}
}