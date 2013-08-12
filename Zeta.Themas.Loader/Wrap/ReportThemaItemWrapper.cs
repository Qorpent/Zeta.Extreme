using Zeta.Themas.Loader.Abstracts;

namespace Zeta.Themas.Loader.Wrap {
	public class ReportThemaItemWrapper : ThemaItemWrapper, IReportThemaItemWrapper {
		protected internal ReportThemaItemWrapper(IThemaItem item, IThemaWrapper wrapper) : base(item, wrapper) {
		}

		#region IReportThemaItemWrapper Members

		public IReportThemaItem Report {
			get { return Item as IReportThemaItem; }
		}

		#endregion
	}
}