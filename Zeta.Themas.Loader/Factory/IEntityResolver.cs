using System;

namespace Zeta.Themas.Loader.Factory {
	public interface IEntityResolver {
		T Get<T>(object key);
		object Get(Type type, object key);
	}
}