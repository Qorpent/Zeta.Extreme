using System;
using Comdiv.Application;
using Comdiv.Inversion;
using Comdiv.Z3.Application;
using InversionExtensions = Comdiv.Inversion.InversionExtensions;

namespace Comdiv.Zeta.Web.Themas {
	public class ThemaFactoryVersionProvider : IThemaFactoryVersionProvider{
		public DateTime GetThemaConfigVersion() {
			return InversionExtensions.get<IThemaFactoryProvider>((IInversionContainer) myapp.ioc).Get().Version;
		}
	}
}