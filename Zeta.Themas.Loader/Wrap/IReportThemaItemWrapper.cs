using Zeta.Themas.Loader.Abstracts;

namespace Zeta.Themas.Loader.Wrap {
	public interface IReportThemaItemWrapper : IThemaItemWrapper {
		IReportThemaItem Report { get; }
	}
}