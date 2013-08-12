using Zeta.Themas.Loader.Abstracts;

namespace Zeta.Themas.Loader.Model.ThemaItemContent {
	internal class FormLockDepend : IFormLockDepend {
		#region IFormLockDepend Members

		public string SourceCode { get; set; }
		public string TargetCode { get; set; }
		public IFormThemaItem Source { get; set; }
		public IFormThemaItem Target { get; set; }

		#endregion
	}
}