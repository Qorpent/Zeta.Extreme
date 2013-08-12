using System.Collections.Generic;
using Zeta.Themas.Loader.Abstracts;
using Zeta.Themas.Loader.Model.ThemaItemContent;

namespace Zeta.Themas.Loader.Model.ThemaItem {
	public class FormThemaItem : ThemaItem, IFormThemaItem {
		public FormThemaItem() {
			InLockDepends = new List<IFormLockDepend>();
			OutLockDepends = new List<IFormLockDepend>();
		}

		#region IFormThemaItem Members

		public string LockCode { get; set; }
		public List<IFormLockDepend> InLockDepends { get; private set; }
		public List<IFormLockDepend> OutLockDepends { get; private set; }

		#endregion

		public override void SetupFromSourceXml() {
			base.SetupFromSourceXml();
			foreach (var e in XmlSource.Elements("lockdepend")) {
				var ld = new FormLockDepend();
				ld.SourceCode = FullCode;
				ld.Source = this;
				ld.TargetCode = e.Attr("code");
				InLockDepends.Add(ld);
			}
		}
	}
}